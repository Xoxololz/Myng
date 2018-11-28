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

        public string DescriptionRarity
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Rarity.GetName() + " " + ItemType.GetName());
                return sb.ToString();
            }
        }

        public string DescriptionAttributes
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (Attributes attribute in Enum.GetValues(typeof(Attributes)))
                {
                    if(attributes.ContainsKey(attribute))
                        sb.Append(attribute.GetName()).Append(" +").Append(attributes[attribute]).AppendLine();
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
        }

        public string DescriptionStats
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = Enum.GetValues(typeof(Stats)).Length - 1; i >= 0; --i)
                {
                    if (stats.ContainsKey((Stats)i)) {
                        Stats stat = (Stats)i;
                        sb.Append(stat.GetName()).Append(" +").Append(stats[stat]).Append(stat.UsesPercentage() ? " %" : "").AppendLine();
                    }
                }
                if(sb.Length > 0)
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
