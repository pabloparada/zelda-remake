using System;
using System.Collections.Generic;
using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Enemies
{
    public class Aquamentus : Enemy
    {
        public Action<List<Weapon>> AddWeaponsToManager;
        public Action<string> RemoveWeaponFromManagerByWeaponId;


        private readonly Direction[] _direction;
        private readonly Vector2 _velocity;
        private readonly List<Weapon> _weapons;
        private readonly Player _player;

        private Direction _targetDirection;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;

        private bool _attacking;

        private Color _lastHitColor;

        public Aquamentus(Vector2 p_position, Player p_player) : base(p_position, new Vector2(16.0f, 16.0f), new Vector2(4.0f, 4.0f))
        {
            life = 6;
            _direction = new[] { Direction.LEFT, Direction.RIGHT };
            _animationController = new AnimationController("Aquamentus");
            _velocity = new Vector2(5.0f, 5.0f);
            _player = p_player;
            _weapons = new List<Weapon>();
            size = new Vector2(32f, 32f);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            var __tmpPosition = position + (_velocity * _targetDirectionVector * p_delta);
            var __reachedTargetPos = ReachedTargetPosition(__tmpPosition, _targetPosition);
            var __isColliding = IsColliding(p_collider, __tmpPosition);

            if (__reachedTargetPos && _targetPosition != Vector2.Zero)
            {
                if (_weapons.Count == 0)
                {
                    var __middleBall = new EnergyBall(this, _player, new Vector2(16, 12.0f));

                    _weapons.Add(__middleBall);
                    _weapons.Add(new EnergyBall(this, _player, new Vector2(16, 12.0f), 10.0f * Main.s_scale));
                    _weapons.Add(new EnergyBall(this, _player, new Vector2(16, 12.0f), -10.0f * Main.s_scale));

                    AddWeaponsToManager.Invoke(_weapons);

                    _attacking = true;
                }
                else
                {
                    for (var __i = _weapons.Count - 1; __i >= 0; __i--)
                    {
                        var __w = _weapons[__i];

                        if (__w.state == State.DISABLED)
                        {
                            RemoveWeaponFromManagerByWeaponId.Invoke(__w.id);
                            _weapons.Remove(__w);
                        }
                    }

                    if (_weapons.Count == 0)
                    {
                        _attacking = false;
                    }
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

        private Direction RevertDirectionEnumerator(Direction p_direction)
        {
            return p_direction == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
        }

        private bool IsColliding(Collider p_collider, Vector2 p_targetPos)
        {
            var __targetAABB = CalculateAABBWithOffset(p_targetPos, hitboxOffset, size);

            return __targetAABB.Min.X <= 144.0f ||
                   p_collider.IsColliding(__targetAABB, _targetDirection);
        }

        private void SortNextMove()
        {
            _targetDirection = position.X <= 144.0f ? RevertDirectionEnumerator(_targetDirection) : _direction[World.s_random.Next(1)];
            
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);

            var __targetDistance = 8.0f * World.s_random.Next(1, 3);

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

        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);

            if (life != 0) return;

            foreach (var __w in _weapons)
            {
                RemoveWeaponFromManagerByWeaponId.Invoke(__w.id);
            }

            _weapons.Clear();
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            if (immunityTimeAferHit >= 0.0f)
            {
                var __currentColor = _animationController.HitColor.Equals(_lastHitColor) ? Color.IndianRed : _animationController.HitColor;

                _animationController.DrawFrame(p_spriteBatch,
                                               MathUtil.GetDrawRectangle(position, size, parentPosition),
                                               __currentColor);

                _lastHitColor = __currentColor;
            }
            else
            {
                _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
            }

            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);
            base.DebugDraw(p_spriteBatch);
        }
    }
}