using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LegendOfZelda.Enemies
{
    public class Zora : Enemy
    {
        private readonly Direction[] _direction;
        private readonly Vector2 _velocity;

        private Direction _targetDirection;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;

        private Player _player;

        private AABB _spawnRegion;

        private float _waitForAttack;

        private bool _attacking;

        public Zora(Vector2 p_position, Collider p_collider, Player p_player) : base(p_position, new Vector2(15.0f, 15.0f), new Vector2(2.0f, 0.0f))
        {
            life = 2;
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _animationController = new AnimationController("Zora");
            _velocity = new Vector2(60.0f, 60.0f);
            _spawnRegion = CalculateSpawnRegion(p_collider);
            _waitForAttack = 0.0f;
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            var __tmpPosition = position + (_velocity * _targetDirectionVector * p_delta);
            var __reachedTargetPos = ReachedTargetPosition(__tmpPosition, _targetPosition);
            var __isColliding = IsColliding(p_collider, __tmpPosition);

            if (__reachedTargetPos && _targetPosition != Vector2.Zero)
            {
                if (weapon == null)
                {
                    InvokeAddWeaponToManager(new DirectionalProjectile(this, new Vector2(5.0f, 5.0f)), "EnergyBall");
                    _attacking = true;
                }
                else if (weapon.state == State.DISABLED)
                {
                    InvokeRemoveWeaponFromManager();
                    _attacking = false;
                }
            }

            if (!_attacking)
            {
                if (__reachedTargetPos || __isColliding)
                {
                    SortNextMove();

                    direction = _targetDirection;
                }
                else
                {
                    position = __tmpPosition;
                }
            }

            base.Update(p_delta, p_collider);
        }

        private AABB CalculateSpawnRegion(Collider p_collider)
        {
            var __spawnRegions = p_collider.FilterCollisionsByCollisionMasks(CollisionMask.WATER);

            var __aabb = new AABB(new Vector2(int.MaxValue, int.MaxValue), new Vector2(int.MinValue, int.MinValue), CollisionMask.WATER);

            for (var __i = 0; __i < __spawnRegions.Count; __i++)
            {
                var __lastMin = __aabb.Min;
                var __currMin = __spawnRegions[__i].Min;

                __aabb.Min = new Vector2(MathHelper.Min(__lastMin.X, __currMin.X), MathHelper.Min(__lastMin.Y, __currMin.Y));

                var __lastMax = __aabb.Min;
                var __currMax = __spawnRegions[__i].Max;

                __aabb.Max = new Vector2(MathHelper.Max(__lastMax.X, __currMax.X), MathHelper.Max(__lastMax.Y, __currMax.Y));
            }

            return __aabb;
        }

        private bool IsColliding(Collider p_collider, Vector2 p_targetPos)
        {
            var __minSpawnRegion = _spawnRegion.Min;
            var __maxSpawnRegion = _spawnRegion.Max;

            var __targetAABB = CalculateAABBWithOffset(p_targetPos, hitboxOffset, size);

            var __minTarget = __targetAABB.Min;
            var __maxTarget = __targetAABB.Max;

            return __minTarget.X <= __minSpawnRegion.X ||
                   __maxTarget.X >= __maxSpawnRegion.X ||
                   __minTarget.Y <= __minSpawnRegion.Y ||
                   __maxTarget.Y >= __maxSpawnRegion.Y;
        }

        private void SortNextMove()
        {
            _targetDirection = _direction[World.s_random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);

            var __targetDistance = 16.0f * World.s_random.Next(1, 3);

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
