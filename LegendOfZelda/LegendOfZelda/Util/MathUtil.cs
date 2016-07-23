using System;
using Microsoft.Xna.Framework;

namespace LegendOfZelda.Util
{
    public class MathUtil
    {
        public static Rectangle GetDrawRectangle(Vector2 p_position, Vector2 p_size, Vector2 p_parentPosition)
        {
            return new Rectangle((int) (p_parentPosition.X + p_position.X) * Main.s_scale, 
                                 (int) (p_parentPosition.Y + p_position.Y) * Main.s_scale,
                                 (int) p_size.X * Main.s_scale, (int) p_size.Y * Main.s_scale);
        }
        public static Rectangle GetDrawRectangle(Rectangle p_rec, Vector2 p_parentPosition)
        {
            return new Rectangle((int) (p_parentPosition.X + p_rec.X) * Main.s_scale,
                                 (int) (p_parentPosition.Y + p_rec.Y) * Main.s_scale,
                                 p_rec.Width * Main.s_scale, p_rec.Height * Main.s_scale);
        }

        public static Vector2 ScaleVectorForDrawing(Vector2 p_vector)
        {
            return new Vector2(p_vector.X * Main.s_scale, p_vector.Y * Main.s_scale + 48 * Main.s_scale);
        }

        public static Vector2 Revert(Vector2 p_vec)
        {
            return p_vec * -Vector2.One;
        }

        public static Vector2 AddHUDMargin(Vector2 p_vec)
        {
            return new Vector2(p_vec.X, p_vec.Y + 48.0f);
        }

        public static Vector2 GetEntityCenter(Vector2 p_position, Vector2 p_size)
        {
            return p_position + p_size * 0.5f;
        }

        public static uint RandomUInt32()
        {
            return (uint)(World.s_random.Next(1 << 30)) << 2 | (uint)(World.s_random.Next(1 << 2));
        }

        public static float Linear(float p_t)
        {
            return p_t;
        }

        public static float EaseInQuad(float p_t)
        {
            return p_t * p_t;
        }

        public static float EaseOutQuad(float p_t)
        {
            return p_t * (2.0f - p_t);
        }

        public static float EaseInOutQuad(float p_t)
        {
            return p_t < 0.5f ? 2.0f * p_t * p_t : -1 + (4.0f - 2.0f * p_t) * p_t;
        }

        public static float EaseInCubic(float p_t)
        {
            return p_t * p_t * p_t;
        }

        public static float EaseOutCubic(float p_t)
        {
            return (--p_t) * p_t * p_t + 1.0f;
        }

        public static float EaseInOutCubic(float p_t)
        {
            return p_t < 0.5f ? 4.0f * p_t * p_t * p_t : (p_t - 1) * (2.0f * p_t - 2.0f) * (2.0f * p_t - 2.0f) + 1.0f;
        }

        public static float EaseInQuart(float p_t)
        {
            return p_t * p_t * p_t * p_t;
        }

        public static float EaseOutQuart(float p_t)
        {
            return 1.0f - (--p_t) * p_t * p_t * p_t;
        }

        public static float EaseInOutQuart(float p_t)
        {
            return p_t < 0.5f ? 8.0f * p_t * p_t * p_t * p_t : 1.0f - 8.0f * (--p_t) * p_t * p_t * p_t;
        }

        public static float EaseInQuint(float p_t)
        {
            return p_t * p_t * p_t * p_t * p_t;
        }

        public static float EaseOutQuint(float p_t)
        {
            return 1.0f + (--p_t) * p_t * p_t * p_t * p_t;
        }

        public static float EaseInOutQuint(float p_t)
        {
            return p_t < 0.5f ? 16.0f * p_t * p_t * p_t * p_t * p_t : 1.0f + 16.0f * (--p_t) * p_t * p_t * p_t * p_t;
        }

        public static float EaseOutBack(float p_t)
        {
            var __v = p_t - 1.0f;
            return __v * __v * ((1.70158f + 1.0f) * __v + 1.70158f) + 1.0f;
        }
    }
}
