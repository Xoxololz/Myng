﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics.Animations;
using Myng.Graphics.Enemies;
using Myng.Helpers;
using Myng.Helpers.Enums;
using Myng.Helpers.SoundHandlers;
using Myng.States;
using System;
using System.Collections.Generic;

namespace Myng.Graphics
{
    abstract public class Character : Sprite
    {
        #region Fields

        protected int health;

        protected int mana;

        protected Vector2 velocity;

        protected float hpScale;

        protected Texture2D hpBar;

        protected SoundEffect2D walkingSound;

        protected Dictionary<Attributes, int> baseAttributes;

        protected float baseSpeed = 3f;

        protected float baseAttackSpeed = 1f;

        protected int basePhysicalDefense = 5;

        protected float baseBlockChance = 0f;

        protected int baseMagicDefense = 5;

        protected float autoAttackTimer;

        protected virtual float AttackSpeed
        {
            get
            {
                return baseAttackSpeed /(1 + (GetAttribute(Attributes.DEXTERITY) / 2)/100f);
            }
        }

        protected virtual float Speed
        {
            get
            {
                return baseSpeed;
            }
        }

        #endregion

        #region Properties

        public Faction Faction { get; set; }

        public int MaxHealth
        {
            get
            {
                return GetAttribute(Attributes.VITALITY) * 10;
            }
        }

        public int MaxMana
        {
            get
            {
                return GetAttribute(Attributes.AURA) * 10;
            }
        }


        public int Health {
            get
            {
                if (health > MaxHealth)
                    health = MaxHealth;
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
                if (mana > MaxMana)
                    mana = MaxMana;
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
                hpScale *= value / scale;
                base.Scale = value;                                
            }
        }

        public virtual int PhysicalDefense
        {
            get
            {
                return basePhysicalDefense + GetAttribute(Attributes.STRENGTH) / 2;
            }
        }

        public virtual int MagicDefense
        {
            get
            {
                return baseMagicDefense + GetAttribute(Attributes.INTELLIGENCE) / 2;
            }
        }

        public virtual float CritChance
        {
            get
            {
                return (float)GetAttribute(Attributes.LUCK) / 2;
            }
        }

        public virtual float BlockChance
        {
            get
            {
                return baseBlockChance;
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
            layer = Layers.Character;            
            hpBar = State.Content.Load<Texture2D>("GUI/HPBar");
            walkingSound = new SoundEffect2D(SoundsDepository.walking.CreateInstance(), this)
            {
                Volume = 0.3f,
                IsLooping = true,
                DistanceDivider = 20
            };

            //inicialize base attributes
            baseAttributes = new Dictionary<Attributes, int>();
            foreach(Attributes stat in Enum.GetValues(typeof(Attributes)))
            {
                baseAttributes.Add(stat, 10);
            }

            Health = MaxHealth;
            Mana = MaxMana;
            autoAttackTimer = baseAttackSpeed;
        }

        public virtual int GetAttribute(Attributes attribute)
        {
            if (!baseAttributes.TryGetValue(attribute, out int result))
            {
                return 0;
            }
            return result;
        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            HandleWalkingSound();

            if (Health <= 0)
            {
                if(this is Enemy enemy)
                {
                    Game1.Player.XP += enemy.XPDrop;
                    ToRemove = true;
                }                
            }
        }

        private void HandleWalkingSound()
        {
            walkingSound.Update3DEffect();
            if(velocity != Vector2.Zero)
            {
                PlayWalkingSound();
            }
            else
            {
                walkingSound.Pause();
            }
        }

        private void PlayWalkingSound()
        {
            if (walkingSound.State == SoundState.Paused)
            {
                walkingSound.Resume();
            }
            else if (walkingSound.State == SoundState.Stopped)
            {
                walkingSound.Play();
            }
        }

        protected bool CheckCollisionWithTerrain()
        {
            return GameState.TileMap.CheckCollisionWithTerrain(CollisionPolygon) == Collision.Solid
                || GameState.TileMap.CheckCollisionWithTerrain(CollisionPolygon) == Collision.Water;

        }

        protected bool DealWithPrimitiveCollisions(List<Sprite> hittableSprites)
        {
            //--------------------------------------------------------------//
            //TODO:deal with crashing into moving trget, probly find how exactly far u can go a go there, not just check
            //if can go as far as velocity lenght. And base total collision on angle.
            //--------------------------------------------------------------//
            Vector2 velocityCopy = new Vector2(velocity.X, velocity.Y);
            velocity = new Vector2(velocityCopy.X, 0);
            if (!CollidesWithNewPosition(hittableSprites) && velocity.Length() > 0.1f)
            {
                Position += velocity;
                return true;
            }
            else
            {
                velocity = new Vector2(0, velocityCopy.Y);
                if (!CollidesWithNewPosition(hittableSprites) && velocity.Length() > 0.1f)
                {
                    Position += velocity;
                    return true;
                }
            }
            velocity = velocityCopy;
            return false;
        }


        protected abstract bool CollidesWithNewPosition(List<Sprite> hittableSprites);
        

        protected abstract bool CheckCollisions(List<Sprite> hittableSprites);

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawFrame(spriteBatch);
            DrawHPBar(spriteBatch);
            if (animationManager != null)
                animationManager.Draw(spriteBatch, Scale, layer);
            else if (texture != null)
                spriteBatch.Draw(texture: texture,position: GlobalOrigin,sourceRectangle: null,color: Color.White,
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
