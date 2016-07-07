using Microsoft.Xna.Framework;

namespace LegendOfZelda
{
    public class Enemy : Entity
    {
        protected AABB _aabb;
        protected Vector2 _position;
        protected Vector2 _size;

        public Enemy(Vector2 p_position, Vector2 p_size)
        {
            state = State.ACTIVE;

            _size = p_size;
            _position = CenterInTile(p_position);
            _position.Y += 48;
            _aabb = new AABB(_position, _position + p_size);
        }

        protected Vector2 CenterInTile(Vector2 p_position)
        {
            return p_position - (_size * 0.55f) + (Vector2.One * 8.0f);
        }
    }
}
