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

namespace Myng.Graphics
{
    public class Projectile : Sprite, ICloneable
    {
        #region Properties

        public Faction Faction { get; set; }

        public Vector2 Direction;
        public float Speed = 6f;
        public double Angle = 0;
        public int Damage = 10;

        #endregion

        #region Fields
        protected float timer = 0f;
        protected float lifespan = 3f;

        protected SoundEffect2D flyingSound;
        protected SoundEffect2D hitSound;

        #endregion

        #region Constructors

        public Projectile(Texture2D texture2D, Vector2 position): base(texture2D, position)
        {
            layer = Layers.Projectile;
            Angle = Math.Atan(Direction.Y / Direction.X);
        }

        public Projectile(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            layer = Layers.Projectile;
            Angle = Math.Atan(Direction.Y / Direction.X);            
        }

        #endregion

        #region Methods

        public virtual void Initialize(Vector2 position, int damage, Vector2 direction, Faction faction, double angle
            , SoundEffectInstance flyingSoundInstance, SoundEffectInstance hitSoundInstance)
        {
            Position = position;
            Damage = damage;
            Direction = direction;
            Direction.Normalize();
            Faction = faction;
            Angle = angle;
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
                        characerSprite.Health -= Damage;
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
                    rotation: (float)Angle, origin: Origin, scale: Scale,
                    effects: SpriteEffects.None, layerDepth: 0);
            else animationManager.Draw(spriteBatch, Scale, Angle, layer);
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
