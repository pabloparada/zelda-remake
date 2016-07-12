using Microsoft.Xna.Framework;

namespace LegendOfZelda.Util
{
    public class MathUtil
    {
        public static Rectangle GetDrawRectangle(Vector2 p_position, Vector2 p_size, Vector2 p_parentPosition)
        {
            return new Rectangle((int)(p_parentPosition.X + p_position.X) * Main.s_scale, 
                (int)(p_parentPosition.Y + p_position.Y) * Main.s_scale,
                (int)p_size.X * Main.s_scale, (int)p_size.Y * Main.s_scale);
        }

        public static Vector2 AddHUDMargin(Vector2 p_vec)
        {
            return new Vector2(p_vec.X, p_vec.Y + 48.0f);
        }
    }
}
