using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class GraphicAssets
    {
        public static Texture2D dungeonMiniMap;
        public static Texture2D guiItemsBackground;
        public static Texture2D guiInventorySmallItemFrame;
        public static Texture2D guiInventoryBigItemFrame;
        public static Texture2D guiInventoryBigTriforce;

        public static SpriteFont zeldaFont12;

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

            zeldaFont12 = Main.s_game.Content.Load<SpriteFont>("ZeldaFont12");

            guiGreenColor = new Color(128f / 256f, 208f / 256f, 16f / 256f);
            guiRedColor = new Color(216f / 256f, 40f / 256f, 0f);
            guiGrayColor = new Color(116f / 256f, 116f / 256f, 116f / 256f);
        }
    }
}
