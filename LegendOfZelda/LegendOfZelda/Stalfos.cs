using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Stalfos : Enemy
    {
        private readonly Direction[] _direction;
        private readonly Vector2 _velocity;
        private readonly AABB _tmpAABB;

        private Direction _targetDirection;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;
        

        public Stalfos(Vector2 p_position) : base(p_position, new Vector2(15.0f, 15.0f))
        {
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _animationController = new AnimationController("Stalfos");
            _velocity = new Vector2(35.0f, 35.0f);
            _tmpAABB = new AABB();
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            var __tmpPosition = position + (_velocity * _targetDirectionVector * p_delta);

            if (ReachedTargetPosition(__tmpPosition, _targetPosition) || IsColliding(p_collider, __tmpPosition))
            {
                SortNextMove();
            }
            else
            {
                position = __tmpPosition;

                aabb.Min = position;
                aabb.Max = position + size;
            }

            base.Update(p_delta);
        }

        private bool IsColliding(Collider p_collider, Vector2 p_targetPos)
        {
            var __isOutOfRange = IsBoundary(p_targetPos);

            _tmpAABB.Min = p_targetPos;
            _tmpAABB.Max = p_targetPos + size;

            var __collisionFound = p_collider.IsColliding(_tmpAABB, _targetDirection);

            return __collisionFound || __isOutOfRange;
        }

        private void SortNextMove()
        {
            _targetDirection = _direction[World.random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);

            var __targetDistance = 16.0f * World.random.Next(1, 5);

            _targetPosition = position + (_targetDirectionVector * __targetDistance);
        }

        public bool ReachedTargetPosition(Vector2 p_pos, Vector2 p_target)
        {
            if (p_target == Vector2.Zero) return true;

            var __dist = (p_pos - p_target).Length();

            return __dist >= -0.5f && __dist <= 0.5f;
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