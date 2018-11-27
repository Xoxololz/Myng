using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;
using Myng.States;
using System.Collections.Generic;
using System.IO;

namespace Myng.PlayerIdentity
{
    abstract public class Identity
    {
        #region Fields
        protected Dictionary<ItemRarity, List<Texture2D>> helmets = new Dictionary<ItemRarity, List<Texture2D>>();
        protected Dictionary<ItemRarity, List<Texture2D>> armors = new Dictionary<ItemRarity, List<Texture2D>>();
        protected Dictionary<ItemRarity, List<Texture2D>> legs = new Dictionary<ItemRarity, List<Texture2D>>();
        protected Dictionary<ItemRarity, List<Texture2D>> weapons = new Dictionary<ItemRarity, List<Texture2D>>();
        protected Dictionary<ItemRarity, List<Texture2D>> shields = new Dictionary<ItemRarity, List<Texture2D>>();
        protected Dictionary<ItemRarity, List<Texture2D>> trinkets = new Dictionary<ItemRarity, List<Texture2D>>();
        #endregion

        #region Properties
        public abstract Attributes PrimaryAttribute
        {
            get;
        }

        public abstract int HPModifier
        {
            get;
        }

        public abstract string Name
        {
            get;
        }
        #endregion

        #region Constructors
        public Identity()
        {
            List<Texture2D> commonTrinkets = LoadListContent("Items/trinket/common");
            List<Texture2D> rareTrinkets = LoadListContent("Items/trinket/rare");
            List<Texture2D> epicTrinkets = LoadListContent("Items/trinket/epic");
            List<Texture2D> legendaryTrinkets = LoadListContent("Items/trinket/legendary");

            trinkets.Add(ItemRarity.COMMON, commonTrinkets);
            trinkets.Add(ItemRarity.RARE, rareTrinkets);
            trinkets.Add(ItemRarity.EPIC, epicTrinkets);
            trinkets.Add(ItemRarity.LEGENDARY, legendaryTrinkets);
        }
        #endregion

        #region Methods
        public List<Texture2D> GetListOfItemTextures(ItemType type, ItemRarity rarity)
        {
            switch (type)
            {
                case ItemType.HELMET:
                    return helmets[rarity];
                case ItemType.CHEST:
                    return armors[rarity];
                case ItemType.LEGS:
                    return legs[rarity];
                case ItemType.WEAPON:
                    return weapons[rarity];
                case ItemType.SHIELD:
                    return shields[rarity];
                case ItemType.TRINKET:
                    return trinkets[rarity];
                default: return null;
            }
        }

        protected List<Texture2D> LoadListContent(string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(State.Content.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            List<Texture2D> result = new List<Texture2D>();

            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);

                result.Add(State.Content.Load<Texture2D>(contentFolder + "/" + name));
            }
            return result;
        }


        #endregion
    }
}
