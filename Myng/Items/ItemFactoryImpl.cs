
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;
using Myng.Items.Interfaces;
using Myng.PlayerIdentity;
using Myng.States;
using System;
using System.Collections.Generic;

namespace Myng.Items
{
    class ItemFactoryImpl : IItemFactory
    {
        #region Fields
        private Random random = new Random();
        private Player player;
        #endregion

        #region Methods
        public void SetPlayer(Player p)
        {
            player = p;
        }

        public Armor CreateRandomArmor(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            List<Texture2D> textures = player.Identity.GetListOfItemTextures(ItemType.CHEST, (ItemRarity)rarity);
            Armor armor = new Armor(textures[random.Next(textures.Count)], (ItemRarity)rarity);
            armor.SetStats(GenerateStats((ItemRarity)rarity, armor.ItemType));
            armor.SetAttributes(GenerateAttributes((ItemRarity)rarity));
            return armor;
        }

        public HealthPotion CreateHealthPotion()
        {
            return new HealthPotion(State.Content.Load<Texture2D>("Items/HealthPotion"));
        }

        public Helmet CreateRandomHelmet(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            List<Texture2D> textures = player.Identity.GetListOfItemTextures(ItemType.HELMET, (ItemRarity)rarity);
            Helmet helmet = new Helmet(textures[random.Next(textures.Count)], (ItemRarity)rarity);
            helmet.SetStats(GenerateStats((ItemRarity)rarity, helmet.ItemType));
            helmet.SetAttributes(GenerateAttributes((ItemRarity)rarity));
            return helmet;
        }

        public Legs CreateRandomLegs(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            List<Texture2D> textures = player.Identity.GetListOfItemTextures(ItemType.LEGS, (ItemRarity)rarity);
            Legs legs = new Legs(textures[random.Next(textures.Count)], (ItemRarity)rarity);
            legs.SetStats(GenerateStats((ItemRarity)rarity, legs.ItemType));
            legs.SetAttributes(GenerateAttributes((ItemRarity)rarity));
            return legs;
        }

        public ManaPotion CreateManaPotion()
        {
            return new ManaPotion(State.Content.Load<Texture2D>("Items/ManaPotion"));
        }

        public Trinket CreateRandomTrinket(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            List<Texture2D> textures = Game1.Player.Identity.GetListOfItemTextures(ItemType.TRINKET, (ItemRarity)rarity);
            Trinket trinket = new Trinket(textures[random.Next(textures.Count)], (ItemRarity)rarity);
            trinket.SetStats(GenerateStats((ItemRarity)rarity, trinket.ItemType));
            trinket.SetAttributes(GenerateAttributes((ItemRarity)rarity));
            return trinket;
        }

        public Shield CreateRandomShield(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            List<Texture2D> textures = Game1.Player.Identity.GetListOfItemTextures(ItemType.SHIELD, (ItemRarity)rarity);
            Shield shield = new Shield(textures[random.Next(textures.Count)], (ItemRarity)rarity);
            shield.SetStats(GenerateStats((ItemRarity)rarity, shield.ItemType));
            shield.SetAttributes(GenerateAttributes((ItemRarity)rarity));
            return shield;
        }

        public Weapon CreateRandomWeapon(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            List<Texture2D> textures = Game1.Player.Identity.GetListOfItemTextures(ItemType.WEAPON, (ItemRarity)rarity);
            Weapon weapon = new Weapon(textures[random.Next(textures.Count)], (ItemRarity)rarity);
            weapon.SetStats(GenerateStats((ItemRarity)rarity, weapon.ItemType));
            weapon.SetAttributes(GenerateAttributes((ItemRarity)rarity));
            return weapon;
        }

        //doesnt create potions
        public Item CreateRandomItem(ItemRarity? rarity = null)
        {
            if (rarity is null)
                rarity = GenerateRarity();
            ItemType itemType = (ItemType)(random.Next(6));

            switch (itemType)
            {
                case ItemType.WEAPON:
                    return CreateRandomWeapon(rarity);
                case ItemType.SHIELD:
                    return CreateRandomShield(rarity);
                case ItemType.TRINKET:
                    return CreateRandomTrinket(rarity);
                case ItemType.LEGS:
                    return CreateRandomLegs(rarity);
                case ItemType.HELMET:
                    return CreateRandomHelmet(rarity);
                case ItemType.CHEST:
                    return CreateRandomArmor(rarity);
                default: throw new ArgumentException();
            }
        }

        public ItemSprite CreateItemSprite(Vector2 position, Item item)
        {
            return new ItemSprite(item.Texture, position, item);
        }

        private ItemRarity GenerateRarity()
        {
            double x = random.NextDouble();

            if (x < 0.5)
                return ItemRarity.COMMON;

            if (x < 0.8)
                return ItemRarity.RARE;

            if (x < 0.95)
                return ItemRarity.EPIC;

            return ItemRarity.LEGENDARY;
        }

        private Dictionary<Attributes, int> GenerateAttributes(ItemRarity rarity)
        {
            Dictionary<Attributes, int> result = new Dictionary<Attributes, int>();
            int amount = GetAttributesAmount(rarity);

            //higher chance for primary attributes based on rarity
            if (rarity == ItemRarity.LEGENDARY || (rarity == ItemRarity.EPIC && random.NextDouble() > 0.3)
                || ((rarity == ItemRarity.RARE && random.NextDouble() < 0.3)))
            {
                result.Add(player.Identity.PrimaryAttribute, GenerateAttributeValue());
                amount--;
            }

            while (amount > 0)
            {
                Attributes attr = (Attributes)(random.Next(Enum.GetValues(typeof(Attributes)).Length));
                if (result.ContainsKey(attr))
                    continue;
                else
                {
                    result.Add(attr, GenerateAttributeValue());
                    amount--;
                }
            }

            return result;
        }

        private Dictionary<Stats, int> GenerateStats(ItemRarity rarity, ItemType type)
        {
            Dictionary<Stats, int> result = new Dictionary<Stats, int>();
            int amount = GetStatsAmount(rarity);

            switch (type)
            {
                case ItemType.WEAPON:
                    result.Add(Stats.MIN_DAMAGE, GetWeaponMinDamage(rarity));
                    result.Add(Stats.MAX_DAMAGE, GetWeaponMaxDamage(rarity));
                    amount--;
                    break;
                case ItemType.SHIELD:
                    result.Add(Stats.PHYSICAL_DEFENSE, GetDefense(rarity));
                    result.Add(Stats.BLOCK, GetBlockChance(rarity, type));
                    amount--;
                    break;
                case ItemType.POTION:
                case ItemType.TRINKET:
                    break;
                case ItemType.LEGS:
                    result.Add((Stats)(random.Next(4, 6)), GetDefense(rarity));
                    result.Add(Stats.MOVEMENT_SPEED, GetMovementSpeed(rarity, type));
                    amount--;
                    break;
                case ItemType.HELMET:
                    result.Add((Stats)(random.Next(4, 6)), GetDefense(rarity));
                    break;
                case ItemType.CHEST:
                    result.Add((Stats)(random.Next(4, 6)), GetDefense(rarity));
                    break;
                default: break;
            }

            while (amount > 0)
            {
                Stats stat = (Stats)(random.Next(6));
                if (result.ContainsKey(stat))
                    continue;
                else
                {
                    result.Add(stat, GenerateStatValue(stat, rarity, type));
                    amount--;
                }
            }

            return result;
        }

        private int GenerateStatValue(Stats stat, ItemRarity rarity, ItemType itemType)
        {
            switch (stat)
            {
                case Stats.ATTACK_SPEED: return GetAttackSpeed(rarity);
                case Stats.CRIT: return GetCriticalChance(rarity);
                case Stats.MOVEMENT_SPEED: return GetMovementSpeed(rarity, itemType);
                case Stats.BLOCK: return GetBlockChance(rarity, itemType);
                case Stats.PHYSICAL_DEFENSE: return GetDefense(rarity);
                case Stats.MAGIC_DEFENSE: return GetDefense(rarity);
                default: return 0;
            }
        }

        private int GetCriticalChance(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.COMMON: return random.Next(1, 6);
                case ItemRarity.RARE: return random.Next(3, 8);
                case ItemRarity.EPIC: return random.Next(5, 11);
                case ItemRarity.LEGENDARY: return random.Next(6, 13);
                default: return 0;
            }
        }

        private int GetAttackSpeed(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.COMMON: return random.Next(1, 11);
                case ItemRarity.RARE: return random.Next(5, 16);
                case ItemRarity.EPIC: return random.Next(15, 26);
                case ItemRarity.LEGENDARY: return random.Next(20, 36);
                default: return 0;
            }
        }

        private int GetDefense(ItemRarity rarity)
        {
            //create better scaling for other classes when we make some
            int bonus = player.Identity is Mage ? player.Level : throw new ArgumentException();
            int baseDef = player.Identity is Mage ? 5 : throw new ArgumentException();
            switch (rarity)
            {
                case ItemRarity.COMMON: return random.Next(baseDef + bonus, baseDef + bonus * 2 + 1);
                case ItemRarity.RARE: return random.Next(baseDef + bonus, (int)(baseDef + bonus * 2.5f) + 1);
                case ItemRarity.EPIC: return random.Next((int)(baseDef + bonus * 1.5f), (int)(baseDef + bonus * 2.5f) + 1);
                case ItemRarity.LEGENDARY: return random.Next((int)(baseDef + bonus * 2), (int)(baseDef + bonus * 3f) + 1);
                default: return 0;
            }
        }

        private int GetMovementSpeed(ItemRarity rarity, ItemType itemType)
        {
            if (itemType == ItemType.LEGS)
            {
                switch (rarity)
                {
                    case ItemRarity.COMMON: return random.Next(5, 16);
                    case ItemRarity.RARE: return random.Next(10, 21);
                    case ItemRarity.EPIC: return random.Next(15, 26);
                    case ItemRarity.LEGENDARY: return random.Next(20, 31);
                    default: return 0;
                }
            }
            else
            {
                switch (rarity)
                {
                    case ItemRarity.COMMON: return random.Next(1, 9);
                    case ItemRarity.RARE: return random.Next(3, 11);
                    case ItemRarity.EPIC: return random.Next(5, 13);
                    case ItemRarity.LEGENDARY: return random.Next(6, 16);
                    default: return 0;
                }
            }
        }

        private int GetBlockChance(ItemRarity rarity, ItemType type)
        {
            if (type == ItemType.SHIELD)
            {
                switch (rarity)
                {
                    case ItemRarity.COMMON: return random.Next(5, 16);
                    case ItemRarity.RARE: return random.Next(10, 21);
                    case ItemRarity.EPIC: return random.Next(15, 26);
                    case ItemRarity.LEGENDARY: return random.Next(20, 26);
                    default: return 0;
                }
            }
            else
            {
                switch (rarity)
                {
                    case ItemRarity.COMMON: return random.Next(1, 4);
                    case ItemRarity.RARE: return random.Next(2, 5);
                    case ItemRarity.EPIC: return random.Next(3, 6);
                    case ItemRarity.LEGENDARY: return random.Next(5, 8);
                    default: return 0;
                }
            }
        }

        private int GetWeaponMinDamage(ItemRarity rarity)
        {
            int bonus = player.Level;
            int baseDMG = 10;
            switch (rarity)
            {
                case ItemRarity.COMMON: return random.Next(baseDMG + bonus * 2, baseDMG + bonus * 3 + 1);
                case ItemRarity.RARE: return random.Next(baseDMG + bonus * 2, (int)(baseDMG + bonus * 3.5f) + 1);
                case ItemRarity.EPIC: return random.Next((int)(baseDMG + bonus * 2.5f), (int)(baseDMG + bonus * 3.5f) + 1);
                case ItemRarity.LEGENDARY: return random.Next((int)(baseDMG + bonus * 2.5f), (int)(baseDMG + bonus * 4f) + 1);
                default: return 0;
            }
        }

        private int GetWeaponMaxDamage(ItemRarity rarity)
        {
            int bonus = player.Level;
            int baseDMG = 25;
            switch (rarity)
            {
                case ItemRarity.COMMON: return random.Next(baseDMG + bonus * 2, baseDMG + bonus * 3 + 1);
                case ItemRarity.RARE: return random.Next(baseDMG + bonus * 2, (int)(baseDMG + bonus * 3.5f) + 1);
                case ItemRarity.EPIC: return random.Next((int)(baseDMG + bonus * 2.5f), (int)(baseDMG + bonus * 3.5f) + 1);
                case ItemRarity.LEGENDARY: return random.Next((int)(baseDMG + bonus * 2.5f), (int)(baseDMG + bonus * 4f) + 1);
                default: return 0;
            }
        }

        private int GenerateAttributeValue()
        {
            int min = player.Level;
            int max = (int)Math.Ceiling(player.Level * 5/3m);
            return random.Next(min, max + 1);
        }

        private int GetAttributesAmount(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.COMMON: return 1;
                case ItemRarity.RARE: return 2;
                case ItemRarity.EPIC: return 2;
                case ItemRarity.LEGENDARY: return 3;
                default: return 0;
            }
        }

        private int GetStatsAmount(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.COMMON: return 1;
                case ItemRarity.RARE: return random.Next(1, 3);
                case ItemRarity.EPIC: return random.Next(2, 4);
                case ItemRarity.LEGENDARY: return 3;
                default: return 0;
            }
        }
        #endregion
    }
}
