using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LegendOfZelda
{
    public class Gel : Enemy
    {
        private Direction[] _direction;
        private Vector2 _velocity;
        private float _targetDistance;
        private float _tick = 3.0f;
        private Vector2 _startPosition;
        private Direction _targetDirection;
        private Vector2 _targetDirectionVector;

        public Gel(Vector2 p_position) : base(p_position, new Vector2(7.0f, 7.0f))
        {
            _velocity = new Vector2(4.0f, 4.0f);
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _startPosition = p_position;
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (_tick > 1.5f)
            {
                _targetDistance = 16 * World.random.Next(1, 2);
                _targetDirection = _direction[World.random.Next(4)];
                _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);
                _startPosition = _position;
                _tick = -0.5f;
            }
            else if(_tick > 0.0f)
            {
                _tick += p_delta;

                var __tmpPosition = InterpolatePosition(_tick);
                var __tmpAABB = new AABB(__tmpPosition, __tmpPosition + _size);

                if (p_collider.IsColliding(__tmpAABB, _targetDirection))
                {
                    _tick = 1.51f;
                }

                _position = __tmpPosition;

                _aabb.Min = _position;
                _aabb.Max = _position + _size;
            }
            else
            {
                _tick += p_delta;
            }

            base.Update(p_delta);
        }

        private Vector2 InterpolatePosition(float tick)
        {
            return Vector2.Lerp(_startPosition, _startPosition + (_targetDistance * _targetDirectionVector), tick);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(_aabb.ScaledRectangleFromAABB(_size), Color.Bisque);
            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            base.DebugDraw(p_spriteBatch);
        }
    }
}
