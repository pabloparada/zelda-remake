﻿using System;
using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class Boomerang : Weapon
    {
        private readonly Vector2 _velocity;
        private readonly Vector2 _targetDirection;
        private readonly Vector2 _reverseTargetDirection;
        private readonly Vector2 _targetPosition;
        private readonly float _maxDistance;

        private float _tick;

        public bool switchedDirection { get; private set; }

        public Boomerang(Entity p_source, float p_maxDistance = 70.0f) : base(p_source, new Vector2(5.0f, 3.0f), p_source.direction)
        {
            weaponType = WeaponType.BOOMERANG;
            size = new Vector2(16.0f, 16.0f);
            hitboxOffset = new Vector2(4.0f, 5.0f);

            _velocity = new Vector2(140.0f, 140.0f);
            _maxDistance = p_maxDistance;
            _tick = 0.0f;
            switchedDirection = false;

            _targetDirection = InputManager.GetDirectionVectorByDirectionEnum(direction);
            _reverseTargetDirection = MathUtil.Revert(_targetDirection);
            _targetPosition = CalculateTargetPosition();

            _animationController = new AnimationController("Boomerang");
            _animationController.ChangeAnimation(InputManager.GetAnimationNameByDirection(p_source.direction));
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (_tick >= 1.0f) _tick = 0.0f;

            var __direction = switchedDirection ? _reverseTargetDirection : _targetDirection;
            var __tmpPosition = CalculatePositionByDirection(switchedDirection, p_delta, __direction);

            if (!switchedDirection)
            {
                _tick = 1.0f - Vector2.Distance(__tmpPosition, _targetPosition) / _maxDistance;
            }
            else
            {
                _tick += p_delta;
            }

            if (ShouldSwitchDirection(__tmpPosition))
            {
                switchedDirection = true;
                _tick = 0.0f;
            }
            else if (switchedDirection && BoomerangArrived(p_collider, __tmpPosition))
            {
                DestroyEntity();
                state = State.DISABLED;
            }
            else
            {
                position = __tmpPosition;
            }

            base.Update(p_delta, p_collider);
        }

        private bool ShouldSwitchDirection(Vector2 p_position)
        {
            return !switchedDirection &&
                   (ReachedTargetPosition(p_position, _targetPosition) || !IsAtScreenBoundaries(p_position, size));
        }

        private bool BoomerangArrived(Collider p_collider, Vector2 p_position)
        {
            var __tmpAABB = CalculateAABBWithOffset(p_position, hitboxOffset, size);

            return p_collider.IsPointInsideRectangle(__tmpAABB.Max, source.position, source.position + source.size)  || 
                   p_collider.IsPointInsideRectangle(__tmpAABB.Min, source.position, source.position + source.size);
        }

        public Vector2 Revert(Vector2 p_vec)
        {
            return p_vec * - Vector2.One;
        }

        private bool ReachedTargetPosition(Vector2 p_pos, Vector2 p_targetPos)
        {
            var __dist = IsHorizontalMovement() ? p_pos.X - p_targetPos.X : p_pos.Y - p_targetPos.Y;

            return __dist >= -1.0f && __dist <= 1.0f;
        }

        private Vector2 CalculatePositionByDirection(bool p_switchedDirection, float p_delta, Vector2 p_direction)
        {
            if (Math.Abs(_tick) < 0.0f) return position;

            if (p_switchedDirection)
            {
                var __dif = MathUtil.GetEntityCenter(source.position, source.size);

                __dif.Y -= size.Y * 0.5f;

                var __direction = (__dif - position);

                __direction.Normalize();

                return position + __direction * Vector2.One * 150.0f * p_delta;
            }

            if (_tick >= 0.2f)
            {
                return position + _velocity * MathUtil.EaseOutQuint(1.0f - _tick) * p_direction * p_delta;
            }

            return position + _velocity  * p_direction * p_delta;
        }

        private Vector2 CalculateTargetPosition()
        {
            return source.position + (Vector2.One * _maxDistance) * _targetDirection;
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition));

            base.Draw(p_spriteBatch);
        }

        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);

            if (p_entity.type == EntityType.ENEMY && !switchedDirection && source.type != EntityType.ENEMY)
            {
                state = State.DISABLED;
            }
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);

            base.DebugDraw(p_spriteBatch);
        }
    }
}