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
    public class GUIMap : Entity
    {
        public float mapCounter = 0f;

        public override void Update(float p_delta)
        {
            base.Update(p_delta);
            mapCounter += p_delta;
            if (mapCounter >= 1f)
                mapCounter -= 1f;
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);
            int __x = 0;
            int __y = 0;
            if (World.mapName.StartsWith("Room"))
            {
                __x = int.Parse(World.mapName.Substring(5, 1));
                __y = int.Parse(World.mapName.Substring(7, 1));
                //BG
                p_spriteBatch.FillRectangle(new Rectangle(16 * Main.s_scale, ((int)position.Y + 188) * Main.s_scale,
                    64 * Main.s_scale, 32 * Main.s_scale), GraphicAssets.guiGrayColor);

                //Player
                p_spriteBatch.FillRectangle(new Rectangle((16 + __x * 4) * Main.s_scale, 
                    ((int)position.Y + 188 + __y * 4) * Main.s_scale,4 * Main.s_scale, 4 * Main.s_scale), 
                    new Color(128f / 256f, 208f / 256f, 16f / 256f));
            }
            else if (World.mapName.StartsWith("Sword"))
            {
                //BG
                p_spriteBatch.FillRectangle(new Rectangle(16 * Main.s_scale, ((int)position.Y + 188) * Main.s_scale,
                    64 * Main.s_scale, 32 * Main.s_scale), GraphicAssets.guiGrayColor);

                //Player
                p_spriteBatch.FillRectangle(new Rectangle(44 * Main.s_scale, 216 * Main.s_scale, 
                    4 * Main.s_scale, 4 * Main.s_scale), GraphicAssets.guiGreenColor);
            }
            else
            {
                __x = int.Parse(World.mapName.Substring(8, 1));
                __y = int.Parse(World.mapName.Substring(10, 1));

                //MiniMap with Map
                p_spriteBatch.Draw(GraphicAssets.dungeonMiniMap, new Rectangle(16 * Main.s_scale, 
                    ((int)parentPosition.Y + 188) * Main.s_scale, 64 * Main.s_scale, 32 * Main.s_scale), Color.White);

                //Draw Map Name
                p_spriteBatch.DrawString(GraphicAssets.zeldaFont12, "LEVEL-1",
                    new Vector2((parentPosition.X + 13f) * Main.s_scale, (parentPosition.Y + 183f) * Main.s_scale), Color.White,
                    0f, Vector2.Zero, Main.s_scale / 2f, SpriteEffects.None, 0f);

                //Player
                p_spriteBatch.FillRectangle(new Rectangle((26 + __x * 8) * Main.s_scale, 
                    ((int)parentPosition.Y +192 + __y * 4) * Main.s_scale,3 * Main.s_scale, 3 * Main.s_scale), 
                    GraphicAssets.guiGreenColor);

                //Triforce with Compass
                if (mapCounter <= 0.5f)
                    p_spriteBatch.FillRectangle(new Rectangle(66 * Main.s_scale, ((int)parentPosition.Y + 196) * Main.s_scale,
                        3 * Main.s_scale, 3 * Main.s_scale), GraphicAssets.guiRedColor);

               
            }

        }
    }
}
