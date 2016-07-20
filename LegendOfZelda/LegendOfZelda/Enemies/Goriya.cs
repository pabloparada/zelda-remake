using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Enemies
{
    public class Goriya : Enemy
    {
        private readonly Direction[] _direction;
        private readonly Vector2 _velocity;
        private readonly AABB _tmpAABB;

        private Direction _targetDirection;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;

        private bool _throwingBoomerang;

        public Goriya(Vector2 p_position) : base(p_position, new Vector2(15.0f, 15.0f), new Vector2(2.0f, 0.0f))
        {
            life = 3;
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _animationController = new AnimationController("Goriya");
            _velocity = new Vector2(35.0f, 35.0f);
            _tmpAABB = new AABB();
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (!isStunned)
            {
                var __tmpPosition = position + (_velocity * _targetDirectionVector * p_delta);
                var __reachedTargetPos = ReachedTargetPosition(__tmpPosition, _targetPosition);
                var __isColliding = IsColliding(p_collider, __tmpPosition);

                if (__reachedTargetPos && _targetPosition != Vector2.Zero)
                {
                    if (weapon == null)
                    {
                        InvokeAddWeaponToManager(WeaponType.BOOMERANG);
                        _throwingBoomerang = true;
                    }
                    else if (weapon.state == State.DISABLED)
                    {
                        InvokeRemoveWeaponFromManager();
                        _throwingBoomerang = false;
                    }
                }

                if (!_throwingBoomerang)
                {
                    if (__reachedTargetPos || __isColliding)
                    {
                        SortNextMove();

                        direction = _targetDirection;
                        _animationController.ChangeAnimation(GetAnimationNameByDirection(_targetDirection));
                    }
                    else
                    {
                        position = __tmpPosition;

                        aabb.Min = position;
                        aabb.Max = position + size;
                    }
                }
            }

            base.Update(p_delta, p_collider);
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
            _targetDirection = _direction[World.s_random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);

            var __targetDistance = 16.0f * World.s_random.Next(1, 12);

            _targetPosition = position + (_targetDirectionVector * __targetDistance);
        }

        public string GetAnimationNameByDirection(Direction p_direction)
        {
            var __name = _targetDirection.ToString();

            return char.ToUpper(__name[0]) + __name.Substring(1).ToLower();
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
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);
            base.DebugDraw(p_spriteBatch);
        }
    }
}