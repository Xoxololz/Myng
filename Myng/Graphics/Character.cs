using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics.Enemies;
using Myng.Helpers;
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

        #endregion

        #region Constructor

        public Character(Texture2D texture2D, Vector2 position)
            : base(texture2D, position)
        {
            Health = MaxHealth;
            Mana = MaxMana;
            layer = Layers.Character;
        }

        public Character(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            Health = MaxHealth;
            Mana = MaxMana;
            layer = Layers.Character;
        }

        #endregion

        #region Methods

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
            //TryMove();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (animationManager != null)
                animationManager.Draw(spriteBatch, Scale, layer);
            else if (texture != null)
                spriteBatch.Draw(texture: texture,position: Position + Scale*Origin,sourceRectangle: null,color: Color.White,
                    rotation: 0,origin: Origin,scale: Scale,effects: SpriteEffects.None,layerDepth: layer);
            else throw new Exception("No texture or animation manager set for Character");
        }

        #endregion
    }
}
