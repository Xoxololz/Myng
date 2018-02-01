using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics.Enemies;
using Myng.Helpers;
using Myng.Helpers.Enums;
using Myng.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Myng.Graphics
{
    abstract public class Character : Sprite
    {
        #region Fields

        protected float speed = 4f;

        protected int health;

        protected int mana;

        protected Vector2 velocity;

        protected float hpScale;

        protected Texture2D hpBar;

        protected float scale;
        #endregion

        #region Properties

        public Faction Faction { get; set; }

        public int MaxHealth = 100;
        public int MaxMana = 100;

        public int Health {
            get
            {
                return health;
            }
            set
            {
                if (value > MaxHealth)
                {
                    health = MaxHealth;
                    return;
                }

                health = value;
            }
        }

        public int Mana
        {
            get
            {
                return mana;
            }
            set
            {
                if (value > MaxMana)
                {
                    mana = MaxMana;
                    return;
                }

                mana = value;
            }
        }

        public override float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                hpScale *= value/scale;
                scale = value;                
            }
        }

        #endregion

        #region Constructor

        public Character(Texture2D texture2D, Vector2 position)
            : base(texture2D, position)
        {
            InitProperties();
            hpScale = (texture.Width * Scale) / hpBar.Width;            
        }

        public Character(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            InitProperties();
            hpScale = (animationManager.Animation.FrameWidth * Scale) / hpBar.Width ;
        }

        #endregion

        #region Methods
        private void InitProperties()
        {
            Health = MaxHealth;
            Mana = MaxMana;
            layer = Layers.Character;            
            hpBar = State.Content.Load<Texture2D>("GUI/HPBar");            
        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites, TileMap tileMap)
        {
            if (Health <= 0)
            {
                if(this is Enemy enemy)
                {
                    Game1.Player.XP += enemy.XPDrop;
                }
                ToRemove = true;
            }
        }

        protected bool CheckCollisionWithTerrain(TileMap tileMap)
        {
            return tileMap.CheckCollisionWithTerrain(CollisionPolygon) == Collision.Solid || tileMap.CheckCollisionWithTerrain(CollisionPolygon) == Collision.Water;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawFrame(spriteBatch);
            DrawHPBar(spriteBatch);
            if (animationManager != null)
                animationManager.Draw(spriteBatch, Scale, layer);
            else if (texture != null)
                spriteBatch.Draw(texture: texture,position: Position + Scale*Origin,sourceRectangle: null,color: Color.White,
                    rotation: 0,origin: Origin,scale: Scale,effects: SpriteEffects.None,layerDepth: layer);
            else throw new Exception("No texture or animation manager set for Character");
        }

        private void DrawHPBar(SpriteBatch spriteBatch)
        {
            Vector2 hpPosition = Position - new Vector2(0, hpBar.Height * hpScale + 6);
            Rectangle hpSource = new Rectangle(0, 0, (int)((hpBar.Width * Health) / MaxHealth), hpBar.Height);
            spriteBatch.Draw(texture: hpBar, position: hpPosition + hpScale * animationManager.Animation.FrameOrigin, sourceRectangle: hpSource, color: Color.White,
                   rotation: 0, origin: animationManager.Animation.FrameOrigin, scale: hpScale, effects: SpriteEffects.None, layerDepth: Layers.Character);
        }

        #endregion
    }
}
