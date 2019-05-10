using Microsoft.Xna.Framework;
using Myng.Graphics;
using Myng.Helpers.Enums;

namespace Myng.Items.Interfaces
{
    interface IItemFactory
    {
        Armor CreateRandomArmor(ItemRarity? rarity = null);
        Trinket CreateRandomTrinket(ItemRarity? rarity = null);
        Helmet CreateRandomHelmet(ItemRarity? rarity = null);
        Legs CreateRandomLegs(ItemRarity? rarity = null);
        Weapon CreateRandomWeapon(ItemRarity? rarity = null);
        Shield CreateRandomShield(ItemRarity? rarity = null);
        HealthPotion CreateHealthPotion();
        ManaPotion CreateManaPotion();
        Item CreateRandomItem(ItemRarity? rarity = null);
        ItemSprite CreateItemSprite(Vector2 position, Item item);
        void SetPlayer(Player player);
    }
}
