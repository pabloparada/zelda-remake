﻿
namespace LegendOfZelda.Items
{
    public class Item : Entity
    {
        public enum SpawnType
        {
            ALWAYS,
            ALL_DEAD
        }
        public SpawnType spawn = SpawnType.ALWAYS;
       
        public enum ItemType
        {
            HEART_CONTAINER,
            KEY,
            COMPASS,
            WOOD_SWORD,
            HEART,
            RUPEE,
            TRIFORCE,
            MAP,
            BOW,
            BOMB,
            BOOMERANG,
            FIRE
        }

        public static Item SpawnItem(Object p_itemObj)
        {
            if (p_itemObj.properties.Name == "Fire")
                return new Fire(p_itemObj);
            else if (p_itemObj.properties.Name == "Map")
                return new MapItem(p_itemObj);
            else if (p_itemObj.properties.Name == "Boomerang")
                return new BoomerangItem(p_itemObj);
            else if (p_itemObj.properties.Name == "Key")
                return new Key(p_itemObj);
            else if (p_itemObj.properties.Name == "Compass")
                return new Compass(p_itemObj);
            else if (p_itemObj.properties.Name == "Bow")
                return new BowItem(p_itemObj);
            else if (p_itemObj.properties.Name == "HeartContainer")
                return new HeartContainer(p_itemObj);
            else if (p_itemObj.properties.Name == "Triforce")
                return new Triforce(p_itemObj);
            else if (p_itemObj.properties.Name == "Rupee")
                return new Rupee(p_itemObj);
            else if (p_itemObj.properties.Name == "Heart")
                return new Heart(p_itemObj);
            else if (p_itemObj.properties.Name == "WoodSword")
                return new WoodSwordItem(p_itemObj);
            else if (p_itemObj.properties.Name == "Oldman")
                return new Oldman(p_itemObj);
            else if (p_itemObj.properties.Name == "Text")
                return new Text(p_itemObj);
            else return null;
        }

        public virtual void AllDead() { }
    }
}
