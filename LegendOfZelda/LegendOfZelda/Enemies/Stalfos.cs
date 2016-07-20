using System;
using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Enemies
{
    public class Stalfos : Enemy
    {
        private readonly Direction[] _direction;
        private readonly Vector2 _velocity;
        
        private Direction _targetDirection;
        private Direction _hitDirection;

        private AABB _tmpAABB;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;
        private Vector2 _hitPushDistance;

        private bool _hitted;

        private float _hittedTimer;

        public Stalfos(Vector2 p_position) : base(p_position, new Vector2(16.0f, 16.0f), new Vector2(2.0f, 2.0f))
        {
            life = 2;
            _direction = new[] { Direction.LEFT, Direction.UP, Direction.DOWN, Direction.RIGHT };
            _animationController = new AnimationController("Stalfos");
            _velocity = new Vector2(35.0f, 35.0f);
            _tmpAABB = new AABB();
            _hitPushDistance = Vector2.Zero;
            _hittedTimer = 0.0f;
            _hitted = false;
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (!isStunned)
            {
                if (_hitted)
                {
                    var __tmpPosition = Vector2.Lerp(position, _hitPushDistance, _hittedTimer);

                    if (ReachedTargetPosition(__tmpPosition, _hitPushDistance) || IsColliding(p_collider, __tmpPosition) || _hittedTimer >= 1.0f)
                    {
                        _hitted = false;
                        _hittedTimer = 0.0f;
                    }
                    else
                    {
                        position = __tmpPosition;
                    }

                    _hittedTimer += p_delta * 0.3f;
                }
                else
                {
                    var __tmpPosition = position + (_velocity * _targetDirectionVector * p_delta);

                    if (ReachedTargetPosition(__tmpPosition, _targetPosition) || IsColliding(p_collider, __tmpPosition))
                    {
                        SortNextMove();
                    }
                    else
                    {
                        position = __tmpPosition;
                    }
                }
            }

            base.Update(p_delta, p_collider);
        }

        private bool IsColliding(Collider p_collider, Vector2 p_targetPos)
        {
            var __isOutOfRange = IsBoundary(p_targetPos);

            _tmpAABB = CalculateAABBWithOffset(p_targetPos, hitboxOffset, size);

            var __collisionFound = p_collider.IsColliding(_tmpAABB, _hitted ? _hitDirection : _targetDirection);

            return __collisionFound || __isOutOfRange;
        }

        private void SortNextMove()
        {
            _targetDirection = _direction[World.s_random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);

            var __targetDistance = 16.0f * World.s_random.Next(1, 5);

            _targetPosition = position + (_targetDirectionVector * __targetDistance);
        }

        public bool ReachedTargetPosition(Vector2 p_pos, Vector2 p_target)
        {
            if (p_target == Vector2.Zero) return true;

            var __dist = (p_pos - p_target).Length();

            return __dist >= -0.5f && __dist <= 0.5f;
        }

        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);

            if (p_entity.type == EntityType.WEAPON && !isStunned && hittedBy != WeaponType.BOOMERANG)
            {
                var __weaponDirection = InputManager.GetDirectionVectorByDirectionEnum(p_entity.direction);
                _hitted = true;
                _hitDirection = p_entity.direction;
                _hitPushDistance = position + __weaponDirection * Vector2.One * 60.0f;
            }
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