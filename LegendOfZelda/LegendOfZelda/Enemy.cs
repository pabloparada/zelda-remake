using Microsoft.Xna.Framework;

namespace LegendOfZelda
{
    public class Enemy : Entity
    {
        public Enemy(Vector2 p_position, Vector2 p_size)
        {
            state = State.ACTIVE;
            size = p_size;
            position = CenterInTile(p_position);
            aabb = new AABB(position, position + p_size);
        }

        protected Vector2 CenterInTile(Vector2 p_position)
        {
            return p_position - (size * 0.55f) + (Vector2.One * 8.0f);
        }

        protected bool IsBoundary(Vector2 p_pos)
        {
            var __line = (int)((p_pos.Y) / 16.0f);
            var __col = (int)(p_pos.X / 16.0f);

            return __col >= 13 || __col <= 1 || __line <= 1 || __line >= 9;
        }
    }
}
