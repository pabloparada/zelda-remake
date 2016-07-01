﻿using Microsoft.Xna.Framework;

namespace LegendOfZelda
{
    public class Projectile : Entity
    {
        public Direction direction { get; set; }
        public bool alive { get; set; }
        public Vector2 position { get; set; }

        protected float _cooldown;
        protected float _maxCooldown;
        protected bool _switchedComponents;
        protected Vector2 initialPlayerPosition;

        public Projectile(Vector2 p_position, Direction p_direction)
        {
            position = initialPlayerPosition = p_position;
            direction = p_direction;
            alive = true;
            _cooldown = 0.0f;
            _maxCooldown = 0.0f;
        }

        public virtual void Update(float delta, Collider p_collider, Vector2 p_playerPosition)
        {
            if (!alive) _switchedComponents = false;

            base.Update(delta, p_collider);
        }

        protected bool IsVerticalMovement()
        {
            return direction == Direction.LEFT || direction == Direction.RIGHT;
        }

        protected Vector2 GetInitialPositionByDirection(Vector2 p_playerSize, Vector2 p_projectileSize)
        {
            return GetInitialPositionByDirection(position, p_playerSize, p_projectileSize);
        }

        public Vector2 GetInitialPositionByDirection(Vector2 p_position, Vector2 p_playerSize, Vector2 p_projectileSize)
        {
            var __initialPosition = new Vector2();

            if (direction == Direction.UP)
            {
                __initialPosition.Y = p_position.Y - p_projectileSize.Y * 0.8f;
                __initialPosition.X = p_position.X + p_playerSize.X * 0.5f - p_projectileSize.X * 0.5f;
            }
            else if (direction == Direction.DOWN)
            {
                __initialPosition.Y = p_position.Y + p_playerSize.Y - p_projectileSize.Y + p_projectileSize.Y * 0.8f;
                __initialPosition.X = p_position.X + p_playerSize.X * 0.5f - p_projectileSize.X * 0.5f;
            }
            else if (direction == Direction.RIGHT)
            {
                __initialPosition.Y = p_position.Y + p_playerSize.Y * 0.5f - p_projectileSize.Y * 0.5f;
                __initialPosition.X = p_position.X + p_playerSize.X - p_projectileSize.X + p_projectileSize.X * 0.8f;
            }
            else if (direction == Direction.LEFT)
            {
                __initialPosition.Y = p_position.Y + p_playerSize.Y * 0.5f - p_projectileSize.Y * 0.5f;
                __initialPosition.X = p_position.X - p_projectileSize.X * 0.8f;
            }

            return __initialPosition;
        }

        public Vector2 Switch(Vector2 p_v1)
        {
            _switchedComponents = true;
            return new Vector2(p_v1.Y, p_v1.X);
        }

        public Vector2 GetProjectileSizeAndControlComponentSwitch(Vector2 p_projectileSize)
        {
            return !IsVerticalMovement() && !_switchedComponents ? Switch(p_projectileSize) : p_projectileSize;
        }

        public Vector2 RevertDirection(Vector2 p_direction)
        {
            return p_direction * -Vector2.One;
        }

        public bool IsCooldownUp()
        {
            return _cooldown >= _maxCooldown;
        }
    }
}
