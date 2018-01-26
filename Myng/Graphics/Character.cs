﻿using Microsoft.Xna.Framework;
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

        //point the character rotates around
        protected Vector2 origin;

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
            origin = new Vector2(texture.Width * Scale / 2, texture.Height * Scale / 2);
        }

        public Character(Dictionary<string, Animation> animations, Vector2 position) : base(animations, position)
        {
            Health = MaxHealth;
            Mana = MaxMana;
            origin = new Vector2(animations.First().Value.FrameWidth * Scale / 2, animations.First().Value.FrameHeight * Scale / 2);
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (animationManager != null)
                animationManager.Draw(spriteBatch, Scale, layer);
            else if (texture != null)
                spriteBatch.Draw(texture: texture,position: Position,sourceRectangle: null,color: Color.White,
                    rotation: 0,origin: origin,scale: Scale,effects: SpriteEffects.None,layerDepth: layer);
            else throw new Exception("No texture or animation manager set for Character");
        }

        #endregion
    }
}
