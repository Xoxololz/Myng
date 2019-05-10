using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics.Animations;
using Myng.Helpers;
using Myng.Helpers.Enums;
using Myng.Helpers.SoundHandlers;
using Myng.States;
using System;
using System.Collections.Generic;
using static Myng.Graphics.Character;

namespace Myng.Graphics
{
    public class Projectile : Sprite, ICloneable
    {
        #region Properties

        public Faction Faction { get; set; }

        public DamageType DamageType { get; set; }

        public Vector2 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                value.Normalize();
                direction = value;
            }
        }
        public float Speed = 6f;
        public double Angle = 0;
        public double AngleTextureOffset { get; set; }
        public int Damage = 10;

        #endregion

        #region Fields
        protected float timer = 0f;
        protected float lifespan = 3f;

        protected SoundEffect2D flyingSound;
        protected SoundEffect2D hitSound;

        protected Character Parent;
        private Vector2 direction;
        #endregion

        #region Constructors

        public Projectile(Texture2D texture2D, Vector2 position): base(texture2D, position)
        {
            layer = Layers.Projectile;
            Angle = Math.Atan(Direction.Y / Direction.X);
            AngleTextureOffset = 0;
        }

        public Projectile(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            layer = Layers.Projectile;
            Angle = Math.Atan(Direction.Y / Direction.X);            
            AngleTextureOffset = 0;
        }

        #endregion

        #region Methods

        public virtual void Initialize(Vector2 position, int damage, DamageType damageType, Vector2 direction, Faction faction
            , SoundEffectInstance flyingSoundInstance, SoundEffectInstance hitSoundInstance, Character parent = null)
        {
            Position = position;
            Damage = damage;
            DamageType = damageType;
            Direction = direction;
            Direction.Normalize();
            Angle = Math.Atan(Direction.Y / Direction.X);            
            Faction = faction;
            Parent = parent;

            flyingSound = new SoundEffect2D(flyingSoundInstance, this)
            {
                IsLooping = true,
                Volume = 0.4f
            };
            hitSound = new SoundEffect2D(hitSoundInstance, this)
            {
                IsLooping = false,
                Volume = 0.5f
            };            
            flyingSound.Play();
        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            UpdateTimer(gameTime);
            flyingSound.Update3DEffect();
            CheckLifespan();
            HandleAnimation(gameTime);
            Move();
            CheckCollisions(hittableSprites);
        }

        private void UpdateTimer(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void CheckLifespan()
        {
            if (timer > lifespan)
            {
                ToRemove = true;
                flyingSound.Stop();
            }
        }

        private void HandleAnimation(GameTime gameTime)
        {
                animationManager?.Update(gameTime);
        }

        private void Move()
        {
            Position += Direction * Speed;
            CollisionPolygon.Translate(Direction * Speed);
        }

        private void CheckCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                CheckCollision(sprite);
            }
            CheckCollision(Game1.Player);
            CheckCollisionWithTerrain();
        }        

        private void CheckCollision(Sprite sprite)
        {
            if (CollisionPolygon.Intersects(sprite.CollisionPolygon))
            {
                if (sprite is Character characerSprite)
                {
                    if(characerSprite.Faction != this.Faction)
                    {
                        flyingSound.Stop();
                        hitSound.Update3DEffect();
                        hitSound.Play();
                        int actualDamage = CalculateDamage(Damage, DamageType, characerSprite, out bool isCrit, out bool isBlocked);
                        characerSprite.Health -= actualDamage;

                        if (isBlocked)
                            characerSprite.CollisionDisplaytList.Add(new CollisionToDisplay("blocked", Color.Snow));
                        else if (isCrit)
                            characerSprite.CollisionDisplaytList.Add(new CollisionToDisplay("*" + actualDamage.ToString() + "*", Color.Red));
                        else
                            characerSprite.CollisionDisplaytList.Add(new CollisionToDisplay(actualDamage.ToString(), Color.OrangeRed));
                        ToRemove = true;
                    }
                }
                else
                {
                    flyingSound.Stop();
                    hitSound.Update3DEffect();
                    hitSound.Play();
                    ToRemove = true;
                }
            }
        }

        private int CalculateDamage(float baseDamage, DamageType damageType, Character targetCharacter, out bool isCrit, out bool isBlocked)
        {
            Random random = new Random();
            //check for block
            double randomBlockNumber = random.NextDouble() * 100;
            isBlocked = randomBlockNumber <= targetCharacter?.BlockChance;

            //check for crit
            double randomCritNumber = random.NextDouble() * 100;
            isCrit = randomCritNumber <= Parent?.CritChance;

            if (isBlocked)
                return 0;

            float damageMultiplier;
            switch (damageType)
            {
                case DamageType.PHYSICAL:
                    damageMultiplier = (baseDamage / (baseDamage + targetCharacter.PhysicalDefense));
                    break;
                case DamageType.MAGIC:
                    damageMultiplier = (baseDamage / (baseDamage + targetCharacter.MagicDefense));
                    break;
                case DamageType.MIXED:
                    //mixed defense is calculated as average of the two defenses minus 1/4 of their difference
                    damageMultiplier = (baseDamage / (baseDamage + ((targetCharacter.PhysicalDefense + targetCharacter.MagicDefense) / 2) - 0.25f*(Math.Abs(targetCharacter.PhysicalDefense - targetCharacter.MagicDefense))));
                    break;
                default: throw new ArgumentException();
            }

            return (int)(baseDamage * damageMultiplier * (isCrit ? 2 : 1));
        }

        private void CheckCollisionWithTerrain()
        {
            if (GameState.TileMap.CheckCollisionWithTerrain(CollisionPolygon) == Collision.Solid)
            {
                ToRemove = true;
                flyingSound.Stop();
                hitSound.Update3DEffect();
                hitSound.Play();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture: texture, position: GlobalOrigin, sourceRectangle: null, color: Color.White,
                    rotation: (float)(Angle + AngleTextureOffset), origin: Origin, scale: Scale,
                    effects: SpriteEffects.None, layerDepth: 0);
            else animationManager.Draw(spriteBatch, Scale, Angle + AngleTextureOffset, layer);
        }

        public void RotateDirection(double angle)
        {
            var x = direction.X * (float)Math.Cos(angle) - direction.Y * (float)Math.Sin(angle);
            var y = direction.X * (float)Math.Sin(angle) + direction.Y * (float)Math.Cos(angle);
            direction.X = x;
            direction.Y = y;
            Angle += angle;
        }

        public virtual object Clone()
        {
            var projectile = this.MemberwiseClone() as Projectile;
            if(animationManager != null)
                projectile.animationManager = animationManager.Clone() as AnimationManager;

            return projectile;
        }

        #endregion
    }
}
