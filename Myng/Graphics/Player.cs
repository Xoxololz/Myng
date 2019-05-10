using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myng.Controller;
using Myng.Helpers;
using Myng.Items.Interfaces;
using Myng.States;
using System;
using System.Collections.Generic;
using Myng.Graphics.Animations;
using Myng.Graphics.GUI;
using Myng.Helpers.Enums;
using Myng.PlayerIdentity;
using Myng.Helpers.Spells;
using Myng.Depositories;

namespace Myng.Graphics
{
    public class Player : Character
    {
        #region Properties

        public Identity Identity { get; private set; }

        public Spellbar Spellbar { get; private set; }

        public Inventory Inventory { get; private set; }

        public Projectile Bullet { get; set; }

        public int CharacterPoints { get; private set; }

        public int XP { get; set; }

        public int NextLevelXP
        {
            get
            {
                return nextLevelXP;
            }
        }

        public int Level
        {
            get
            {
                return level;
            }
        }

        public override int MaxHealth
        {
            get
            {
                return GetAttribute(Attributes.VITALITY) * Identity.HPModifier;
            }
        }

        public override float CritChance
        {
            get
            {
                return ((GetAttribute(Attributes.LUCK) * 2f) / (level + 1)) + Inventory.GetStatBonus(Stats.CRIT);
            }
        }

        public override int PhysicalDefense
        {
            get
            {
                return basePhysicalDefense + GetAttribute(Attributes.STRENGTH) / 2 + Inventory.GetStatBonus(Stats.PHYSICAL_DEFENSE);
            }
        }

        public override int MagicDefense
        {
            get
            {
                return baseMagicDefense + GetAttribute(Attributes.INTELLIGENCE) / 2 + Inventory.GetStatBonus(Stats.MAGIC_DEFENSE);
            }
        }

        public override float BlockChance
        {
            get
            {
                return baseBlockChance + Inventory.GetStatBonus(Stats.BLOCK);
            }
        }

        public override float Speed
        {
            get
            {
                return baseSpeed * MovementSpeedBonus;
            }
        }

        public float MovementSpeedBonus
        {
            get
            {
                return 1 + (Inventory.GetStatBonus(Stats.MOVEMENT_SPEED) / 100f);
            }
        }

        public override float AttackSpeed
        {
            get
            {
                return baseAttackSpeed / (1 + ((GetAttribute(Attributes.DEXTERITY) / 2) + Inventory.GetStatBonus(Stats.ATTACK_SPEED)) / 100f);
            }
        }

        public override int MinDamage
        {
            get
            {
                return Inventory.GetStatBonus(Stats.MIN_DAMAGE) > 0 ? Inventory.GetStatBonus(Stats.MIN_DAMAGE) : baseMinDamage;
            }
        }

        public override int MaxDamage
        {
            get
            {
                return Inventory.GetStatBonus(Stats.MAX_DAMAGE) > 0 ? Inventory.GetStatBonus(Stats.MAX_DAMAGE) : baseMaxDamage;
            }
        }

        public Vector2 AttackDirection;

        public Direction FacingDirection
        {
            get
            {
                switch (animationManager.Animation.CurrentFrame.Y)
                {
                    case 0:
                        return Direction.Down;
                    case 1:
                        return Direction.Left;
                    case 2:
                        return Direction.Right;
                    case 3:
                        return Direction.Up;
                    default:
                        return Direction.Right;
                }
            }
        }
        #endregion

        #region Fields

        private Input input;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private Spell autoAttack;

        private int nextLevelXP;

        private int level;

        private Dictionary<string, Animation> playerAnimations;

        #endregion

        #region Constructor
        public Player(Dictionary<string, Animation> animations, Vector2 position, Identity identity) : base(animations, position)
        {
            currentKey = Keyboard.GetState();
            previousKey = Keyboard.GetState();
            velocity = new Vector2(0f);
            input = new Input();
            Scale = 1.5f;
            baseAttackSpeed = 1f;
            AttackDirection = new Vector2(0, -1);
            Inventory = new Inventory();
            Spellbar = new Spellbar();
            Faction = Faction.FRIENDLY;
            level = 1;
            XP = 0;
            CharacterPoints = 5;
            nextLevelXP = 100;
            this.Identity = identity;
            
            InitAutoattack();
            InitSpells();
        }

        #endregion

        #region Methods

        private void InitSpells()
        {
            //testing function 
            Action<List<Sprite>> spell = (sprites) =>
            {
                var fireballAnimation = new Dictionary<string, Animation>()
                {
                    { "fireball", new Animation(State.Content.Load<Texture2D>("Projectiles/fireball"), 1, 6)
                        {
                            FrameSpeed = 0.05f
                        }
                    }
                };
                var animation = new AnimationSprite(fireballAnimation, GlobalOrigin);
                animation.Position -= animation.Origin * animation.Scale;
                sprites.Add(animation);
            };


            Func<bool> canExecute = () =>
            {
                return true;
            };

            Spellbar.Add(SpellDepository.TrippleFireball(this));
            Spellbar.Add(new Spell(spell, 15, State.Content.Load<Texture2D>("Projectiles/projectile"), 1));
            Spellbar.Add(new Spell(spell, 20, State.Content.Load<Texture2D>("Projectiles/fireball_icon"), 1));
            Spellbar.Add(new Spell(spell, 25, State.Content.Load<Texture2D>("Projectiles/projectile"), 1));
        }

        private void InitAutoattack()
        {
            autoAttack = SpellDepository.RangeAutoAttack(this);
        }

        public override int GetAttribute(Attributes attribute)
        {
            if (!baseAttributes.TryGetValue(attribute, out int result))
            {
                return Inventory == null ? 0 : Inventory.GetAttributeBonus(attribute);
            }
            return result + (Inventory == null ? 0 : Inventory.GetAttributeBonus(attribute));
        }

        public void LevelUp()
        {
            ++level;
            XP = 0;
            nextLevelXP = (int) (100 * Math.Pow(1.25, level));
            CharacterPoints += 3;
        }

        public override void Update(GameTime gameTime, List<Sprite> otherSprites, List<Sprite> hittableSprites)
        {
            UpdateSpellCooldowns(gameTime);
            previousKey = currentKey;
            currentKey = Keyboard.GetState();
            autoAttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // just temporary solution until we somehow handle player dying
            if (Health <= 0)
            {
                Health = MaxHealth;
            }

            if(XP >= nextLevelXP)
            {
                LevelUp();
            }

            Move(hittableSprites);
            HandleAnimation();
            animationManager.Update(gameTime);
            UsePotions(otherSprites);
            CastSpells(otherSprites);

            Inventory.UpdateEquippedItems(otherSprites, hittableSprites);
            
            base.Update(gameTime, otherSprites, hittableSprites);
        }

        private void UpdateSpellCooldowns(GameTime gameTime)
        {
            foreach (var spell in Spellbar.spells)
            {
                spell.UpdateCooldown(gameTime);
            }
            autoAttack.UpdateCooldown(gameTime);
        }

        private void UsePotions(List<Sprite> sprites)
        {
            if (currentKey.IsKeyDown(input.HealthPotion) && !previousKey.IsKeyDown(input.HealthPotion))
            {
                ((IUsable)Inventory.HealthPotion)?.Use(sprites);
            }
            if (currentKey.IsKeyDown(input.ManaPotion) && !previousKey.IsKeyDown(input.ManaPotion))
            {
                ((IUsable)Inventory.ManaPotion)?.Use(sprites);
            }
        }

        private void CastSpells(List<Sprite> sprites)
        {
            if (currentKey.IsKeyDown(input.ShootUp) && currentKey.IsKeyUp(input.ShootDown) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootRight))
            {
                AttackDirection.X = 0;
                AttackDirection.Y = -1;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(3);
            }
            if (currentKey.IsKeyDown(input.ShootDown) && currentKey.IsKeyUp(input.ShootUp) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootRight))
            {
                AttackDirection.X = 0;
                AttackDirection.Y = 1;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(0);
            }
            if (currentKey.IsKeyDown(input.ShootRight) && currentKey.IsKeyUp(input.ShootUp) && currentKey.IsKeyUp(input.ShootLeft) && currentKey.IsKeyUp(input.ShootDown))
            {
                AttackDirection.X = 1;
                AttackDirection.Y = 0;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(2);
            }
            if (currentKey.IsKeyDown(input.ShootLeft) && currentKey.IsKeyUp(input.ShootUp) && currentKey.IsKeyUp(input.ShootDown) && currentKey.IsKeyUp(input.ShootRight))
            {
                AttackDirection.X = -1;
                AttackDirection.Y = 0;
                CastAutoAttack(sprites);
                animationManager.Animation.SetRow(1);
            }

            if (currentKey.IsKeyDown(input.Spell1) && !previousKey.IsKeyDown(input.Spell1))
                Spellbar.GetSpell(0)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell2) && !previousKey.IsKeyDown(input.Spell2))
                Spellbar.GetSpell(1)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell3) && !previousKey.IsKeyDown(input.Spell3))
                Spellbar.GetSpell(2)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell4) && !previousKey.IsKeyDown(input.Spell4))
                Spellbar.GetSpell(3)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell5) && !previousKey.IsKeyDown(input.Spell5))
                Spellbar.GetSpell(4)?.Cast(sprites);
            if (currentKey.IsKeyDown(input.Spell6) && !previousKey.IsKeyDown(input.Spell6))
                Spellbar.GetSpell(5)?.Cast(sprites);
        }

        private void CastAutoAttack(List<Sprite> sprites)
        {           
                autoAttack.Cast(sprites);
        }

        private void Move(List<Sprite> hittableSprites)
        {
            velocity = Vector2.Zero;
            if (currentKey.IsKeyDown(input.Left))
            {
                velocity.X -= 1f;
            }
            if (currentKey.IsKeyDown(input.Right))
            {
                velocity.X += 1f;
            }
            if (currentKey.IsKeyDown(input.Up))
            {
                velocity.Y -= 1f;
            }
            if (currentKey.IsKeyDown(input.Down))
            {
                velocity.Y += 1f;
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                velocity *= Speed;
            }
            
            if (CollidesWithNewPosition(hittableSprites))
            {
                DealWithPrimitiveCollisions(hittableSprites);
            }
            else
            {
                Position += velocity;
            }
        }
        
        protected override bool CollidesWithNewPosition(List<Sprite> hittableSprites)
        {
            Position += velocity;
            if (CheckCollisions(hittableSprites))
            {
                Position -= velocity;
                return true;
            }
            if (CheckCollisionWithTerrain())
            {
                Position -= velocity;
                return true;
            }
            Position -= velocity;
            return false;
        }

        protected override bool CheckCollisions(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (CheckCollision(sprite))
                    return true;
            }
            if (CheckCollisionWithTerrain())
                return true;
            return false;
        }


        private bool CheckCollision(Sprite sprite)
        {
            //int minDistance = 35;
            //if (Vector2.Distance(CollisionPolygon.Origin, sprite.CollisionPolygon.Origin) < minDistance)
            //{
            //    return true;
            //}
            //return false;
            return CollisionPolygon.Intersects(sprite.CollisionPolygon);
        }

        private void HandleAnimation()
        {
            animationManager.Animation.IsLooping = true;
            if (velocity.X > 0)
                animationManager.Animation.SetRow(2); //walking right
            else if (velocity.X < 0)
                animationManager.Animation.SetRow(1); //walking left
            else if (velocity.Y > 0)
                animationManager.Animation.SetRow(0); //walking down
            else if (velocity.Y < 0)
                animationManager.Animation.SetRow(3); //walking up
            else
                animationManager.Animation.IsLooping = false;
            
        }

        public override void ImproveAttribute(Attributes attribute, int amount)
        {
            if (CharacterPoints >= amount)
            {
                base.ImproveAttribute(attribute, amount);
                CharacterPoints -= amount;
            }
        }

        #endregion
    }
}
