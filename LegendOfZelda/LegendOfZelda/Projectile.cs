using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Projectile : Entity
    {
        public Direction direction { get; set; }
        public bool alive { get; set; }
        public Vector2 position { get; set; }

        public Projectile(Vector2 p_position, Direction p_direction)
        {
            position = p_position;
            direction = p_direction;
            alive = true;
        }
    }
}
