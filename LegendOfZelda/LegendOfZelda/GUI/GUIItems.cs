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
                new Vector2(185f * Main.s_scale, ((int)parentPosition.Y + 188) * Main.s_scale), GraphicAssets.guiRedColor, 
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);

            //Draw Rupees
                p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "X" + World.random.Next(0,5),
                    new Vector2((parentPosition.X + 97f) * Main.s_scale, (parentPosition.Y + 188f) * Main.s_scale), Color.White,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);

            //Draw Keys
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "X" + World.random.Next(0, 5),
                new Vector2((parentPosition.X + 97f) * Main.s_scale, (parentPosition.Y + 204f) * Main.s_scale), Color.White,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);

            //Draw Bombs
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "X" + World.random.Next(0, 5),
                new Vector2((parentPosition.X + 97f) * Main.s_scale, (parentPosition.Y + 212f) * Main.s_scale), Color.White,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);

            //Draw Second Item HotKey
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "B",
                new Vector2((parentPosition.X + 129f) * Main.s_scale, (parentPosition.Y + 188f) * Main.s_scale), Color.White,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);
            p_spriteBatch.Draw(TilesetManager.inventoryTileset,
                       new Rectangle((124 + (int)parentPosition.X) * Main.s_scale,
                       (196 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                       TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 7), Color.White);
            //Draw Sword HotKey
            p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "A",
                new Vector2((parentPosition.X + 153f) * Main.s_scale, (parentPosition.Y + 188f) * Main.s_scale), Color.White,
                0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);
            p_spriteBatch.Draw(TilesetManager.inventoryTileset,
                        new Rectangle((148 + (int)parentPosition.X) * Main.s_scale,
                        (196 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                        TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, 9), Color.White);
            //Draw Hearts
            int __x = 0;
            int __type = 0;
            for (int i = 0; i < linkMaxLife; i += 2)
            {
                if (linkLife - i > 1)
                    __type = 0;
                else if (linkLife - i == 1)
                    __type = 1;
                else
                    __type = 2;
                p_spriteBatch.Draw(TilesetManager.inventoryTileset,
                        new Rectangle((176 + (int)parentPosition.X + __x * 8) * Main.s_scale,
                        (208 + (int)parentPosition.Y) * Main.s_scale, 16 * Main.s_scale, 16 * Main.s_scale),
                        TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.INVENTORY, __type), Color.White);
                __x++;
            }
        
        }

        
    }
}
