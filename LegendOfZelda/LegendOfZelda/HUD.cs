using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{ 
    public class HUD
    {
        public Vector2 hudPosition = new Vector2(0f, -176f);

        public void DrawHUD(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(new Rectangle((int)hudPosition.X * Main.s_scale,
                ((int)hudPosition.Y + 176) * Main.s_scale, 256 * Main.s_scale, 48 * Main.s_scale),Color.Black, 2f);
        }
    }
}
