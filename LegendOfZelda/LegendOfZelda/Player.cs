using System;
using System.Globalization;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        private readonly Vector2 _velocity;
        private readonly int _maximumLife;

        private bool _isColliding;
        private bool _hitted;

        private Direction _lasDirection;
        private Direction _hitDirection;

        public AABB movementAABB { get; private set; }

        private Vector2 _directionVector;
        private Vector2 movementAABBOffset;
        private Vector2 movementAABBSize;
        private Vector2 _hitPushDistance;
        private Vector2 _hitPushDirectionVector;

        private Color _lastHitColor;

        private float _hittedTimer;

        public Player()
        {
            type = EntityType.PLAYER;
            tag = "Player";
            name = "Player";
            state = State.ACTIVE;

            life = 16;
            _maximumLife = 16;
            
            position = new Vector2(120, 120);
            size = new Vector2(16, 16);
            direction = GetDefaultDirection();
            _animationController = new Animations.AnimationController("Player");
            for(int i = 4; i < 8; i ++)
                _animationController.AnimationsList[i].OnAnimationEnd += Player_OnAnimationEnd;
            _lasDirection = Direction.DOWN;
            _velocity = new Vector2(80.0f, 80.0f);
            _directionVector = InputManager.GetDirectionVectorByDirectionEnum(direction);

            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
            movementAABBOffset = new Vector2(4f, 7f);
            movementAABBSize = new Vector2(8f, 7f);
            movementAABB = new AABB(position + movementAABBOffset, position + size);
        }

        private void Player_OnAnimationEnd()
        {
            Console.WriteLine("Here");
            _animationController.ChangeAnimation("Walk" 
                + _animationController.Animation.name.Remove(0, 6));
        }

        public void ForcePosition(Vector2 p_pos)
        {
            position = p_pos;
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            direction = GetDefaultDirection();

            var __dir = InputManager.GetDirection().Item1;

            if (__dir == Direction.NONE && _animationController.Animation.name.StartsWith("Walk"))
            {
                animationSpeed = 0f;
            }
            else
            {
                animationSpeed = 1f;
            }

            if (__dir != _lasDirection)
            {
                _animationController.ChangeAnimation("Walk" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(__dir.ToString().ToLower()));

                _directionVector = InputManager.GetDirection().Item2;
            }
            else
            {
                _directionVector = InputManager.GetDirectionVectorByDirectionEnum(_lasDirection);
            }

            MoveAndFixCollisionFraction(p_delta, p_collider, __dir);

            _lasDirection = direction;

            base.Update(p_delta, p_collider);
        }

        private Direction GetDefaultDirection()
        {
            var __dir = InputManager.GetDirection();
            return __dir.Item1 == Direction.NONE ? _lasDirection : __dir.Item1;
        }

        public void MoveAndFixCollisionFraction(float p_delta, Collider p_collider, Direction p_direction)
        {
            if (_hitted && immunityTimeAferHit >= 0.0f)
            {
                var __tmpPosition = Vector2.Lerp(position, _hitPushDistance, _hittedTimer);

                aabb.Min = __tmpPosition;
                aabb.Max = __tmpPosition + size;
                movementAABB.Min = __tmpPosition + movementAABBOffset;
                movementAABB.Max = __tmpPosition + movementAABBOffset + movementAABBSize;

                if (ReachedTargetPosition(__tmpPosition, _hitPushDistance) || 
                    p_collider.IsColliding(movementAABB) ||
                    _hittedTimer >= 1.0f)
                {
                    _hitted = false;
                    _hittedTimer = 0.0f;
                }
                else
                {
                    position = __tmpPosition;
                }

                if (!IsAtScreenBoundaries(__tmpPosition, size))
                {
                    var __dirVec = MathUtil.Revert(_hitPushDirectionVector);

                    direction = InputManager.GetDirectionEnumByDirectionVector(_hitPushDirectionVector);

                    position += new Vector2(3.0f, 3.0f) * __dirVec;

                    _hitted = false;
                    _hittedTimer = 0.0f;
                }

                _hittedTimer += p_delta*0.3f;
            }
            else
            {
                var __maxReach = 0.0f;

                var __tempPos = position + _directionVector * _velocity * p_delta;

                aabb.Min = __tempPos;
                aabb.Max = __tempPos + size;
                movementAABB.Min = __tempPos + movementAABBOffset;
                movementAABB.Max = __tempPos + movementAABBOffset + movementAABBSize;

                if (p_collider.IsColliding(movementAABB))
                {
                    var __reachFraction = p_delta * 0.5f;

                    for (var __i = 0; __i < 4; __i++)
                    {
                        __tempPos = position + (_directionVector * _velocity * p_delta * __reachFraction);

                        _isColliding = p_collider.IsColliding(new AABB(__tempPos + movementAABBOffset, __tempPos + movementAABBOffset + movementAABBSize), p_direction);

                        if (_isColliding)
                            __reachFraction -= 1f / (float)Math.Pow(2, __i + 2);
                        else
                        {
                            __reachFraction += 1f / (float)Math.Pow(2, __i + 2);
                            __maxReach = __reachFraction;
                        }
                    }
                }
                else
                {
                    __maxReach = 1f;
                }

                if (__maxReach > 0f)
                {
                    position += _directionVector * _velocity * p_delta * __maxReach;
                    position = new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y));
                }
            }
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

            if ((p_entity.type == EntityType.ENEMY || p_entity.type == EntityType.WEAPON) && immunityTimeAferHit == -0.5f)
            {
                if (p_entity.type == EntityType.WEAPON)
                {
                    var __weapon = (Weapon) p_entity;

                    if (__weapon.source.type == EntityType.PLAYER || __weapon.weaponType == WeaponType.BOOMERANG) return;
                }

                _hitPushDirectionVector = MathUtil.Revert(InputManager.GetDirectionVectorByDirectionEnum(direction));

                life -= 1;
                _hitted = true;
                _hitPushDistance = position + _hitPushDirectionVector * Vector2.One * 40.0f;
                immunityTimeAferHit = 0.0f;

                SoundManager.instance.Play(life == 0 ? SoundType.PLAYER_DEATH : SoundType.PLAYER_HITTED);
            }
        }

        public bool IsHealthFull()
        {
            return _maximumLife == life;
        }

        public void SetAttackAnimation()
        {
            _animationController.ChangeAnimation("Attack" + _animationController.Animation.name.Remove(0, 4));
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            if (immunityTimeAferHit - 0.2f >= 0.0f)
            {
                var __currentColor = _animationController.HitColor.Equals(_lastHitColor) ? Color.White : _animationController.HitColor;

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
            var __debugHitbox = MathUtil.GetDrawRectangle(position, size, parentPosition);

            p_spriteBatch.DrawRectangle(__debugHitbox, Color.Green);

            if (_isColliding) p_spriteBatch.DrawRectangle(__debugHitbox, Color.Red, 1.0f);

            var __msgPos = new Vector2((parentPosition.X + position.X) * Main.s_scale, (parentPosition.Y + position.Y) * Main.s_scale);

            p_spriteBatch.DrawRectangle(movementAABB.ScaledRectangleFromAABB(), Color.Yellow, 2.0f);

            p_spriteBatch.DrawString(Main.s_debugFont, "X:" + (int) position.X + " Y:" + (int) position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}