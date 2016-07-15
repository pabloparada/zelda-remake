using Microsoft.Xna.Framework;

namespace LegendOfZelda.Weapons
{
    public class Weapon : Entity
    {
        public Direction direction { get; set; }
        public Vector2 position { get; set; }
        public Entity source { get; set; }

        protected float cooldown;
        protected float maxCooldown;
        protected Vector2 initialSourcePosition;

        private bool _switchedComponents;

        public Weapon(Entity p_source, Vector2 p_size, Direction p_direction)
        {
            source = p_source;
            direction = p_direction;
            size = GetProjectileSizeAndControlComponentSwitch(p_size);
            initialSourcePosition = p_source.position;
            position = CenterPositionByDirection(p_source.position, p_source.size, size);
            cooldown = 0.0f;
            maxCooldown = 0.0f;
        }

        protected bool IsHorizontalMovement()
        {
            return direction == Direction.LEFT || direction == Direction.RIGHT;
        }

        protected Vector2 CenterPositionByDirection(Vector2 p_sourceSize, Vector2 p_weaponSize)
        {
            return CenterPositionByDirection(position, p_sourceSize, p_weaponSize);
        }

        public Vector2 CenterPositionByDirection(Vector2 p_position, Vector2 p_sourceSize, Vector2 p_weaponSize)
        {
            var __initialPosition = new Vector2();

            if (direction == Direction.UP)
            {
                __initialPosition.Y = p_position.Y - p_weaponSize.Y * 0.8f;
                __initialPosition.X = p_position.X + p_sourceSize.X * 0.5f - p_weaponSize.X * 0.5f;
            }
            else if (direction == Direction.DOWN)
            {
                __initialPosition.Y = p_position.Y + p_sourceSize.Y - p_weaponSize.Y + p_weaponSize.Y * 0.8f;
                __initialPosition.X = p_position.X + p_sourceSize.X * 0.5f - p_weaponSize.X * 0.5f;
            }
            else if (direction == Direction.RIGHT)
            {
                __initialPosition.Y = p_position.Y + p_sourceSize.Y * 0.5f - p_weaponSize.Y * 0.5f;
                __initialPosition.X = p_position.X + p_sourceSize.X - p_weaponSize.X + p_weaponSize.X * 0.8f;
            }
            else if (direction == Direction.LEFT)
            {
                __initialPosition.Y = p_position.Y + p_sourceSize.Y * 0.5f - p_weaponSize.Y * 0.5f;
                __initialPosition.X = p_position.X - p_weaponSize.X * 0.8f;
            }

            return __initialPosition;
        }

        public bool IsAtScreenBoundaries(Vector2 p_position, Vector2 p_size)
        {
            var __min = p_position;
            var __max = p_position + p_size;

            if (World.IsOpenWorld())
            {
                return __min.X > 0.0f && __min.X < 255.0f && __min.Y > 0.0f && __min.Y < 182.0f &&
                       __max.X > 0.0f && __max.X < 255.0f && __max.Y > 0.0f && __max.Y < 182.0f;
            }
            else
            {
                return __min.X > 24.0f && __min.X < 232.0f && __min.Y > 16.0f && __min.Y < 152.0f &&
                       __max.X > 24.0f && __max.X < 232.0f && __max.Y > 16.0f && __max.Y < 152.0f;
            }
        }

        protected Vector2 Switch(Vector2 p_v1)
        {
            _switchedComponents = true;
            return new Vector2(p_v1.Y, p_v1.X);
        }

        protected Vector2 GetProjectileSizeAndControlComponentSwitch(Vector2 p_projectileSize)
        {
            return IsHorizontalMovement() && !_switchedComponents ? Switch(p_projectileSize) : p_projectileSize;
        }

        public bool IsCooldownUp()
        {
            return cooldown >= maxCooldown;
        }
    }
}
