using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Enemies
{
    public class Gel : Enemy
    {
        private readonly Direction[] _direction;
        private readonly float _timeBetweenMoviment = -1.5f;
        private readonly float _timeBetweenSquares = -0.9f;

        private Direction _targetDirection;
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private Vector2 _targetDirectionVector;

        private int _numSquaresToMove;

        private float _tick = -1.5f;
        
        private AABB _nextMovementAABB;

        public Gel(Vector2 p_position) : base(p_position, new Vector2(16.0f, 16.0f), new Vector2(5.0f, 5.0f))
        {
            animationSpeed = 7.0f;

            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _startPosition = position;
            _targetPosition = position;
            _nextMovementAABB = CalculateAABBWithOffset(position, hitboxOffset, size);
            _animationController = new AnimationController("Gel");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (!isStunned)
            {
                if (_tick >= -0.5f && _tick < 0.0f)
                {
                    _startPosition = position;

                    if (_numSquaresToMove != 0)
                    {
                        _targetPosition = _startPosition + (16.0f*_targetDirectionVector);

                        _numSquaresToMove--;

                        _tick = 0.0f;
                    }
                    else
                    {
                        SortNextMove();

                        _tick = -0.5f;
                    }
                }
                else if (_tick > 0.0f)
                {
                    _nextMovementAABB = CalculateAABBWithOffset(_targetPosition, hitboxOffset, size);
                    var __collisionFound = p_collider.IsColliding(_nextMovementAABB, _targetDirection);

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

                        _tick += p_delta*2.5f;
                    }
                }
                else
                {
                    _tick += p_delta;
                }
            }

            base.Update(p_delta, p_collider);
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
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);
            p_spriteBatch.DrawRectangle(_nextMovementAABB.ScaledRectangleFromAABB(), Color.Black, 2.0f);
            base.DebugDraw(p_spriteBatch);
        }
    }
}
