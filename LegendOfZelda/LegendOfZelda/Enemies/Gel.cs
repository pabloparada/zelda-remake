using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Enemies
{
    public class Gel : Enemy
    {
        private readonly Direction[] _direction;
        private int _numSquaresToMove;
        private float _tick = -1.5f;
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private Direction _targetDirection;
        private Vector2 _targetDirectionVector;

        private float _timeBetweenMoviment = -1.5f;
        private float _timeBetweenSquares = -0.9f;

        public Gel(Vector2 p_position) : base(p_position, new Vector2(6.0f, 6.0f))
        {
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _startPosition = position;
            _targetPosition = position;
            _animationController = new AnimationController("Gel");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (_tick >= -0.5f && _tick < 0.0f)
            {
                _startPosition = position;

                if (_numSquaresToMove != 0)
                {
                    _targetPosition = _startPosition + (16.0f * _targetDirectionVector);

                    _numSquaresToMove--;

                    _tick = 0.0f;
                }
                else
                {
                    SortNextMove();

                    _tick = -0.5f;
                }
            }
            else if(_tick > 0.0f)
            {
                var __tmpAABB = new AABB(_targetPosition, _targetPosition + size);
                var __collisionFound = p_collider.IsColliding(__tmpAABB, _targetDirection);

                if (__collisionFound)
                {
                    _numSquaresToMove = 0;
                }

                if (__collisionFound || _tick >= 1.0f)
                {
                    _tick = _numSquaresToMove != 0 ? _timeBetweenSquares : _timeBetweenMoviment;
                }
                else
                {
                    position = InterpolatePosition(_tick);

                    aabb.Min = position;
                    aabb.Max = position + size;

                    _tick += p_delta * 2.5f;
                }
            }
            else
            {
                _tick += p_delta;
            }

            base.Update(p_delta);
        }

        private void SortNextMove()
        {
            _numSquaresToMove = World.s_random.Next(1, 3);
            _targetDirection = _direction[World.s_random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);
        }

        private Vector2 InterpolatePosition(float p_tick)
        {
            return Vector2.Lerp(_startPosition, _startPosition + (16.0f * _targetDirectionVector), p_tick);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(position, size, parentPosition), Color.Bisque);
            base.DebugDraw(p_spriteBatch);
        }
    }
}
