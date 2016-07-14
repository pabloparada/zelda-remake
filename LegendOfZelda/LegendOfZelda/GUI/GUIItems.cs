using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.GUI
{
    public class GUIItems : Entity
    {
        public int linkMaxLife = 16;
        public int linkLife = 11;

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);

            //Draw Background
            p_spriteBatch.Draw(GraphicAssets.guiItemsBackground, new Rectangle(88 * Main.s_scale, 
                               ((int)parentPosition.Y + 188) * Main.s_scale, 144 * Main.s_scale, 32 * Main.s_scale), Color.White);

            //Draw -LIFE-
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "-LIFE-", 
                                     new Vector2(185f * Main.s_scale, ((int)parentPosition.Y + 188.0f) * Main.s_scale), GraphicAssets.guiRedColor,
                                     0.0f, Vector2.Zero, Main.s_scale / 2.0f, SpriteEffects.None, 0.0f);

            //Draw Rupees
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "X" + Inventory.Instance.rupeeCount,
                                     new Vector2((parentPosition.X + 97.0f) * Main.s_scale, (parentPosition.Y + 188.0f) * Main.s_scale), Color.White,
                                     0.0f, Vector2.Zero, Main.s_scale / 2.0f, SpriteEffects.None, 0.0f);

            //Draw Keys
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "X" + Inventory.Instance.keyCount,
                                     new Vector2((parentPosition.X + 97.0f) * Main.s_scale, (parentPosition.Y + 204.0f) * Main.s_scale), Color.White,
                                     0.0f, Vector2.Zero, Main.s_scale / 2.0f, SpriteEffects.None, 0.0f);

            //Draw Bombs
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "X" + Inventory.Instance.bombCount,
                                     new Vector2((parentPosition.X + 97.0f) * Main.s_scale, (parentPosition.Y + 212.0f) * Main.s_scale), Color.White,
                                     0.0f, Vector2.Zero, Main.s_scale / 2.0f, SpriteEffects.None, 0.0f);

            //Draw Second Item HotKey
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "B",
                                     new Vector2((parentPosition.X + 129.0f) * Main.s_scale, (parentPosition.Y + 188.0f) * Main.s_scale), Color.White,
                                     0f, Vector2.Zero, Main.s_scale / 2.0f, SpriteEffects.None, 0.0f);

            p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                               new Rectangle((124 + (int)parentPosition.X) * Main.s_scale,
                               (196 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                               TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 7), Color.White);

            //Draw Sword HotKey
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "A",
                                     new Vector2((parentPosition.X + 153.0f) * Main.s_scale, (parentPosition.Y + 188.0f) * Main.s_scale), Color.White,
                                     0.0f, Vector2.Zero, Main.s_scale / 2.0f, SpriteEffects.None, 0.0f);

            p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                               new Rectangle((148 + (int)parentPosition.X) * Main.s_scale,
                               (196 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                               TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 9), Color.White);

            //Draw Hearts
            var __x = 0;

            for (var __i = 0; __i < linkMaxLife; __i += 2)
            {
                var __type = 0;

                if (linkLife - __i > 1)
                {
                    __type = 0;
                }
                else if (linkLife - __i == 1)
                {
                    __type = 1;
                }
                else
                {
                    __type = 2;
                }

                p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                                   new Rectangle((176 + (int)parentPosition.X + __x * 8) * Main.s_scale,
                                   (208 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                                   TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, __type), Color.White);

                __x++;
            }
        }
    }
}