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

        private Direction _targetDirection;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;
        private Vector2 _hitPushDistance;

        private bool _throwingBoomerang;
        private bool _hitted;

        private float _hittedTimer;

        private Color _lastHitColor;

        public Goriya(Vector2 p_position) : base(p_position, new Vector2(16.0f, 16.0f), new Vector2(2.0f, 2.0f))
        {
            life = 3;
            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _animationController = new AnimationController("Goriya");
            _velocity = new Vector2(35.0f, 35.0f);
            _hitPushDistance = Vector2.Zero;
            _hittedTimer = 0.0f;
            _hitted = false;
            _lastHitColor = Color.White;
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (!isStunned)
            {
                // dont push when boomerang is being thrown
                if (_hitted && _throwingBoomerang)
                {
                    _hitted = false;
                    _hittedTimer = 0.0f;
                }

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
                    var __reachedTargetPos = ReachedTargetPosition(__tmpPosition, _targetPosition);
                    var __isColliding = IsColliding(p_collider, __tmpPosition);

                    if (__reachedTargetPos && _targetPosition != Vector2.Zero)
                    {
                        if (weapon == null)
                        {
                            InvokeAddWeaponToManager(new Boomerang(this));
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
                            _animationController.ChangeAnimation(InputManager.GetAnimationNameByDirection(_targetDirection));
                        }
                        else
                        {
                            position = __tmpPosition;
                        }
                    }
                }
            }

            base.Update(p_delta, p_collider);
        }

        private bool IsColliding(Collider p_collider, Vector2 p_targetPos)
        {
            var __isOutOfRange = IsBoundary(p_targetPos);

            var __collisionFound = p_collider.IsColliding(CalculateAABBWithOffset(p_targetPos, hitboxOffset, size));

            return __collisionFound || __isOutOfRange;
        }

        private void SortNextMove()
        {
            _targetDirection = _direction[World.s_random.Next(4)];
            _targetDirectionVector = InputManager.GetDirectionVectorByDirectionEnum(_targetDirection);

            var __targetDistance = 16.0f * World.s_random.Next(1, 12);

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
                var __weap = (Weapon) p_entity;

                if (__weap.source.type != EntityType.ENEMY)
                {
                    var __weaponDirection = InputManager.GetDirectionVectorByDirectionEnum(p_entity.direction);
                    _hitted = true;
                    _hitPushDistance = position + __weaponDirection * Vector2.One * 60.0f;
                }
            }
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