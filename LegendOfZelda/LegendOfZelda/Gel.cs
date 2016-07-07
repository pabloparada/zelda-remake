using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Animations;

namespace LegendOfZelda
{
    public class Gel : Enemy
    {
        private Direction[] _direction;
        private Vector2 _velocity;
        private float _targetDistance;
        private float _tick = -1.5f;
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private Direction _targetDirection;
        private Vector2 _targetDirectionVector;

        public Gel(Vector2 p_position) : base(p_position, new Vector2(6.0f, 6.0f))
        {
            _velocity = new Vector2(4.0f, 4.0f);
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _startPosition = position;
            _targetPosition = position;
            animationController = new AnimationController("Gel");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (_tick >= -0.5f && _tick < 0.0f)
            {
                SortNextMove();
                
                _tick = 0.0f;
            }
            else if(_tick > 0.0f)
            {
                if (CollisionDetected(_targetPosition, p_collider) || _tick >= 1.0f)
                {
                    _tick = -1.5f;
                }
                else
                {
                    var __tmpPosition = InterpolatePosition(_targetPosition, _tick);
                    var __tmpAABB = new AABB(__tmpPosition, __tmpPosition + size);

                    position = __tmpPosition;

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

        private bool CollisionDetected(Vector2 p_position, Collider p_collider)
        {
            var __targetPos = new Vector2(p_position.X, p_position.Y);
            var __col = (int) __targetPos.X / 16;
            var __row = (int) __targetPos.Y / 16;

            return __row <= 1 || __row >= 9 || __col <= 1 || __col >= 14;
        }

        private void SortNextMove()
        {
            _targetDistance = 16 * World.random.Next(1, 3);
            _targetDirection = _direction[World.random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);
            _startPosition = position;
            _targetPosition = _startPosition + (_targetDistance * _targetDirectionVector);
        }

        private Vector2 InterpolatePosition(Vector2 p_target, float tick)
        {
            return Vector2.Lerp(_startPosition, _startPosition + (_targetDistance * _targetDirectionVector), tick);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(position, size, parentPosition), Color.Bisque);
            base.DebugDraw(p_spriteBatch);
        }
    }
}
