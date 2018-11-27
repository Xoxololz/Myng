using System;
using Microsoft.Xna.Framework.Graphics;
using Myng.Graphics;
using Myng.Helpers.Enums;
using System.Collections.Generic;
using System.Text;

namespace Myng.Items
{
    public abstract class Item
    {
        #region Properties
        public ItemType ItemType { get; protected set; }

        public ItemRarity Rarity { get; protected set; }

        protected Dictionary<Attributes, int> attributes;

        protected Dictionary<Stats, int> stats;

        //maximum amount of the same the same item to have in inventory
        public int MaxCount
        {
            get
            {
                return maxCount;
            }

            set
            {
                maxCount = value;
            }
        }

        public int Count { get; set; }        

        public Player Parent { get; set; }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public bool BeingDragged { get; set; }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Rarity.GetName() + " " + ItemType.GetName()).AppendLine();
                foreach (Attributes attribute in attributes.Keys)
                {
                    sb.Append(attribute.GetName()).Append(" +").Append(attributes[attribute]).AppendLine();
                }
                foreach (Stats stat in stats.Keys)
                {
                    sb.Append(stat.GetName()).Append(" +").Append(stats[stat]).Append(stat.UsesPercentage() ? " %" : "").AppendLine();
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
        }

        #endregion

        #region Fields

        protected Texture2D texture;

        protected int maxCount = 1;

        #endregion

        #region Constructors

        public Item(Texture2D texture, ItemType itemType, ItemRarity rarity)
        {
            this.texture = texture;
            ItemType = itemType;
            BeingDragged = false;
            Rarity = rarity;
            Count = 1;
        }

        public int GetStat(Stats stat)
        {
            if (!stats.TryGetValue(stat, out int result))
            {
                return 0;
            }
            return result;
        }

        public int GetAttribute(Attributes attribute)
        {
            if (!attributes.TryGetValue(attribute, out int result))
            {
                return 0;
            }
            return result;
        }

        public void SetStats(Dictionary<Stats, int> stats)
        {
            this.stats = stats;
        }

        public void SetAttributes(Dictionary<Attributes, int> attrs)
        {
            this.attributes = attrs;
        }
        #endregion
    }
}
