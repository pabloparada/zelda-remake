using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class TilesetManager
    {
       
        public enum TileSetType
        {
            ITEMS
        }
        public enum ItemTileSet
        {
            HEART_CONTAINER = 0,
            KEY,
            COMPASS,
            WOOD_SWORD,
            HEART1,
            HEART2,
            RUPEE1,
            RUPEE2,
            TRIFORCE1,
            TRIFORCE2,
            MAP,
            BOW,
            BOMB,
            BOOMERANG,
            FIRE1,
            FIRE2
        }


        public static Texture2D itemsTileset { get; protected set; }
        public static void Setup()
        {
            itemsTileset = Main.s_game.Content.Load<Texture2D>("TileSet_Items");
        }

        public static Rectangle GetSourceRectangle(TileSetType p_type, int p_index)
        {
            Rectangle __sourceRect = new Rectangle(0, 0, 16, 16);
            if (p_type == TileSetType.ITEMS)
            {
                __sourceRect.X = (p_index % (itemsTileset.Width / 16)) * 16;
                __sourceRect.Y = (p_index / (itemsTileset.Height / 16)) * 16;
            }
            return __sourceRect;
        }
    }
}
