using Microsoft.Xna.Framework;

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

        protected bool IsVerticalMovement()
        {
            return direction == Direction.LEFT || direction == Direction.RIGHT;
        }

        protected Vector2 GetInitialPositionByDirection(Vector2 p_playerSize, Vector2 p_projectileSize)
        {
            var __initialPosition = new Vector2();

            if (direction == Direction.UP)
            {
                __initialPosition.Y = position.Y + p_playerSize.Y * 0.25f - p_projectileSize.Y;
                __initialPosition.X = position.X + p_playerSize.X * 0.5f - p_projectileSize.X * 0.5f;
            }
            else if (direction == Direction.DOWN)
            {
                __initialPosition.Y = position.Y - p_playerSize.Y * 0.25f + p_projectileSize.Y;
                __initialPosition.X = position.X + p_playerSize.X * 0.5f - p_projectileSize.X * 0.5f;
            }
            else if (direction == Direction.RIGHT)
            {
                __initialPosition.Y = position.Y + p_playerSize.Y * 0.5f - p_projectileSize.Y * 0.5f;
                __initialPosition.X = position.X - p_playerSize.Y * 0.25f + p_projectileSize.X;
            }
            else if (direction == Direction.LEFT)
            {
                __initialPosition.Y = position.Y + p_playerSize.Y * 0.5f - p_projectileSize.Y * 0.5f;
                __initialPosition.X = position.X + p_playerSize.Y * 0.25f - p_projectileSize.X;
            }

            return __initialPosition;
        }
    }
}
