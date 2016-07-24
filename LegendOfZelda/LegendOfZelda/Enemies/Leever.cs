using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Enemies
{
    public enum OktorokType { RED, BLUE }

    public class Leever : Enemy
    {
        private readonly Direction[] _direction;
        private readonly Vector2 _velocity;

        private Direction _targetDirection;
        private Direction _hitDirection;

        private Vector2 _targetDirectionVector;
        private Vector2 _targetPosition;
        private Vector2 _hitPushDistance;

        private float _waitForDig;
        private bool _digging;

        private Color _lastHitColor;

        private bool _hitted;

        private float _hittedTimer;

        public Leever(EnemyType p_enemyType, Vector2 p_position) : base(p_position, new Vector2(15.0f, 15.0f), new Vector2(2.0f, 0.0f))
        {
            life = 2;
            animationSpeed = 2.5f;

            _direction = new[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT };
            _animationController = new AnimationController(EnemyTypeResolver.TypeToString(p_enemyType, "Leever"));
            _animationController.AnimationsList[1].OnAnimationEnd += LeeverOnAnimationEnd;
            _velocity = new Vector2(35.0f, 35.0f);
       
            _waitForDig = 0.0f;
            _digging = true;
            _hitPushDistance = Vector2.Zero;
        }

        private void LeeverOnAnimationEnd()
        {
            _animationController.ChangeAnimation("Leever");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (!isStunned)
            {
                if (_hitted)
                {
                    var __tmpPosition = Vector2.Lerp(position, _hitPushDistance, _hittedTimer);

                    if (ReachedTargetPosition(__tmpPosition, _hitPushDistance) ||
                        IsColliding(p_collider, __tmpPosition) || _hittedTimer >= 1.0f)
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
                    if (!_digging)
                    {

                        var __tmpPosition = position + (_velocity * _targetDirectionVector * p_delta);
                        var __reachedTargetPos = ReachedTargetPosition(__tmpPosition, _targetPosition);
                        var __isColliding = IsColliding(p_collider, __tmpPosition);

                        if (__reachedTargetPos && _targetPosition != Vector2.Zero)
                        {
                            _waitForDig = 0.0f;
                            _digging = true;
                        }
                        else if (__isColliding || __reachedTargetPos)
                        {
                            SortNextMove();
                        }
                        else
                        {
                            position = __tmpPosition;

                        }
                    }
                    else
                    {
                        if (_waitForDig <= 0.8f && _digging)
                        {
                            if (_animationController.Animation.name != "Underground")
                            {
                                _animationController.ChangeAnimation("Underground");
                            }
                        }
                        else if (_waitForDig > 0.8f && _waitForDig <= 1.0f)
                        {
                            _animationController.ChangeAnimation("Emerging");
                        }
                        else
                        {
                            _digging = false;
                            SortNextMove();
                        }

                        _waitForDig += p_delta;
                    }
                }
            }

            direction = _targetDirection;

            base.Update(p_delta, p_collider);
        }

        private bool IsColliding(Collider p_collider, Vector2 p_targetPos)
        {
            return !IsAtScreenBoundaries(p_targetPos, size) || 
                   p_collider.IsColliding(CalculateAABBWithOffset(p_targetPos, hitboxOffset, size), _targetDirection);
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

        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);

            if (p_entity.type == EntityType.WEAPON && !isStunned && hittedBy != WeaponType.BOOMERANG && immunityTimeAferHit >= 0.0f)
            {
                var __weaponDirection = InputManager.GetDirectionVectorByDirectionEnum(p_entity.direction);

                _hitted = true;
                _hitDirection = p_entity.direction;
                _hitPushDistance = position + __weaponDirection * Vector2.One * 60.0f;
            }
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);
            base.DebugDraw(p_spriteBatch);
        }
    }
}
