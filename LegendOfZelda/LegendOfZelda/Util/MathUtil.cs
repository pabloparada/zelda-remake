using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class MathUtil
    {
        public static Rectangle GetDrawRectangle(Vector2 p_position, Vector2 p_size, Vector2 p_parentPosition)
        {
            return new Rectangle((int)(p_parentPosition.X + p_position.X) * Main.s_scale, 
                (int)(p_parentPosition.Y + p_position.Y) * Main.s_scale,
                (int)p_size.X * Main.s_scale, (int)p_size.Y * Main.s_scale);
        }
    }
}
