using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class GraphicAssets
    {
        //Images
        public static Texture2D dungeonMiniMap;
        public static Texture2D guiItemsBackground;
        public static Texture2D guiInventorySmallItemFrame;
        public static Texture2D guiInventoryBigItemFrame;
        public static Texture2D guiInventoryBigTriforce;

        //TileSets
        public static Texture2D itemsTileset { get; protected set; }
        public static Texture2D enemiesTileset { get; protected set; }
        public static Texture2D playerTileset { get; protected set; }
        public static Texture2D projectilesTileset { get; protected set; }
        public static Texture2D inventoryTileset { get; protected set; }

        //Fonts
        public static SpriteFont zeldaFont12;

        //Colors
        public static Color guiGreenColor;
        public static Color guiRedColor;
        public static Color guiGrayColor; 

        public static void LoadContent()
        {
            dungeonMiniMap = Main.s_game.Content.Load<Texture2D>("Dungeon1_MiniMap");
            guiItemsBackground = Main.s_game.Content.Load<Texture2D>("GUIItemsBackground");
            guiInventorySmallItemFrame = Main.s_game.Content.Load<Texture2D>("GUIInventorySmallItemFrame");
            guiInventoryBigItemFrame = Main.s_game.Content.Load<Texture2D>("GUIInventoryBigItemFrame");
            guiInventoryBigTriforce = Main.s_game.Content.Load<Texture2D>("GUIInventoryBigTriforce");

            itemsTileset = Main.s_game.Content.Load<Texture2D>("TileSet_Items");
            enemiesTileset = Main.s_game.Content.Load<Texture2D>("TileSet_Enemies");
            inventoryTileset = Main.s_game.Content.Load<Texture2D>("TileSet_Inventory");

            zeldaFont12 = Main.s_game.Content.Load<SpriteFont>("ZeldaFont12");

            guiGreenColor = new Color(128f / 256f, 208f / 256f, 16f / 256f);
            guiRedColor = new Color(216f / 256f, 40f / 256f, 0f);
            guiGrayColor = new Color(116f / 256f, 116f / 256f, 116f / 256f);
        }
    }
}
