﻿using Myng.Items;
using Myng.Items.Interfaces;
using System.Collections.Generic;

namespace Myng.Helpers
{
    public class Inventory
    {
        #region Properties

        public List<Item> Items;

        #endregion

        #region Fields

        private int maxsize=6;

        #endregion

        #region Constructors

        public Inventory()
        {
            Items = new List<Item>();
        }

        #endregion

        #region Methods

        public bool CanBeAdded(Item item)
        {
            if (!Items.Exists((p) => item.GetType() == p.GetType()))
            {
                //early exit if Inventory is full
                if (Items.Count >= maxsize) return false;

                return true;
            }
            else
            {
                foreach (var a in Items)
                {
                    if (a.GetType() == item.GetType())
                    {
                        //exit if there is already maximum amount of this type of items
                        if (item.Count >= item.MaxCount) return false;

                        return true;
                    }
                }
            }
            return false;
        }

        public bool Add(Item item)
        {
            if (!Items.Exists((p) => item.GetType() == p.GetType()))
            {
                //early exit if Inventory is full
                if (Items.Count >= maxsize) return false;

                IStatImprover statImprover = item as IStatImprover;
                if (statImprover != null)
                    statImprover.ImproveStats();

                Items.Add(item);

                return true;
            }
            else
            {
                foreach (var a in Items)
                {
                    if (a.GetType() == item.GetType())
                    {
                        //exit if there is already maximum amount of this type of items
                        if (item.Count >= item.MaxCount) return false;

                        item.Count++;
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearEmptyItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Count <= 0)
                {
                    Items.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw()
        {

        }

        #endregion
    }
}