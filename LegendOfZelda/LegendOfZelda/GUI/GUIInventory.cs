using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda.GUI
{
    public class GUIInventory : Entity
    {
        public override void Update(float p_delta)
        {
            base.Update(p_delta);
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);

            //Draw Small Item Frame
            p_spriteBatch.Draw(GraphicAssets.guiInventorySmallItemFrame, new Rectangle(56 * Main.s_scale,
                ((int)parentPosition.Y + 39) * Main.s_scale, 32 * Main.s_scale, 32 * Main.s_scale), Color.White);
            //Draw Big Item Frame
            p_spriteBatch.Draw(GraphicAssets.guiInventoryBigItemFrame, new Rectangle(116 * Main.s_scale,
                ((int)parentPosition.Y + 39) * Main.s_scale, 112 * Main.s_scale, 48 * Main.s_scale), Color.White);

            //Draw INVENTORY
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "INVENTORY",
                new Vector2(34f * Main.s_scale, (parentPosition.Y + 23f) * Main.s_scale), GraphicAssets.guiRedColor,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);

            //Draw Use B Button
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "USE B BUTTON\n  FOR THIS",
                new Vector2(16f * Main.s_scale, (parentPosition.Y + 71f) * Main.s_scale), Color.White,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);
            p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                        new Rectangle((63 + (int)parentPosition.X) * Main.s_scale,
                        (47 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                        TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 6), Color.White);

            //Draw Map
            if (Inventory.Instance.hasMap)
            {
                p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "MAP",
                    new Vector2(40f * Main.s_scale, (parentPosition.Y + 95f) * Main.s_scale), GraphicAssets.guiRedColor,
                    0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);
                p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                    new Rectangle((44 + (int)parentPosition.X) * Main.s_scale,
                    (111 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                    TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 3), Color.White);

                //Draw Compass
                if (Inventory.Instance.hasCompass)
                {
                    p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "COMPASS",
                        new Vector2(24f * Main.s_scale, (parentPosition.Y + 135f) * Main.s_scale), GraphicAssets.guiRedColor,
                        0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);
                    p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                        new Rectangle((44 + (int)parentPosition.X) * Main.s_scale,
                        (151 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                        TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 4), Color.White);
                }
            }
            //Draw Big Triforce
            else
            {
                p_spriteBatch.Draw(GraphicAssets.guiInventoryBigTriforce, new Rectangle(80 * Main.s_scale,
                    ((int)parentPosition.Y + 103) * Main.s_scale, 96 * Main.s_scale, 48 * Main.s_scale), Color.White);
                p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "TRIFORCE",
                   new Vector2(97f * Main.s_scale, (parentPosition.Y + 159f) * Main.s_scale), GraphicAssets.guiRedColor,
                   0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);
            }
            //Draw Items
            p_spriteBatch.Draw(GraphicAssets.inventoryTileset,
                        new Rectangle((63 + (int)parentPosition.X) * Main.s_scale,
                        (47 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                        TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 6), Color.White);
        }
    }
}
