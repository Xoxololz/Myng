using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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

        #endregion

        #region Constructors

        public Projectile(Texture2D texture2D, Vector2 position)
            : base(texture2D, position)
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
        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites, TileMap tileMap)
        {
            UpdateTimer(gameTime);
            CheckLifespan();
            HandleAnimation(gameTime);
            Move();
            CheckCollisions(hittableSprites, tileMap);
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
            }
        }

        private void HandleAnimation(GameTime gameTime)
        {
            if (animationManager != null)
            {
                animationManager.Update(gameTime);
            }
        }

        private void Move()
        {
            Position += Direction * Speed;
            CollisionPolygon.Translate(Direction * Speed);
        }

        private void CheckCollisions(List<Sprite> sprites, TileMap tileMap)
        {
            foreach (var sprite in sprites)
            {
                CheckCollision(sprite);
            }
            CheckCollision(Game1.Player);
            CheckCollisionWithTerrain(tileMap);
        }

        private void CheckCollisionWithTerrain(TileMap tileMap)
        {
            if (tileMap.CheckCollisionWithTerrain(CollisionPolygon))
            {
                ToRemove = true;
            }
        }

        private void CheckCollision(Sprite sprite)
        {
            if (CollisionPolygon.Intersects(sprite.CollisionPolygon))
            {
                if (sprite is Character characerSprite)
                {
                    if(characerSprite.Faction != this.Faction)
                    {
                        characerSprite.Health -= Damage;
                        ToRemove = true;
                    }
                }
                else
                {
                    ToRemove = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture: texture, position: Position, sourceRectangle: null, color: Color.White,
                    rotation: (float)Angle, origin: CollisionPolygon.Origin - Position, scale: Scale,
                    effects: SpriteEffects.None, layerDepth: 0);
            else animationManager.Draw(spriteBatch, Scale, Angle, CollisionPolygon.Origin - Position, layer);
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
