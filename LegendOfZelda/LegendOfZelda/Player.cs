using System;
using System.Globalization;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        public int health { get; set; }

        private readonly Vector2 _velocity;
        private readonly int _maximumLife;

        private bool _isColliding;

        private Vector2 _directionVector;
        private Direction _lasDirection;

        private AABB movementAABB;
        private Vector2 movementAABBOffset;
        private Vector2 movementAABBSize;
        public Player()
        {
            type = EntityType.PLAYER;
            tag = "Player";
            name = "Player";
            state = State.ACTIVE;

            health = 16;
            _maximumLife = 16;
            
            position = new Vector2(120, 120);
            size = new Vector2(16, 16);
            direction = GetDefaultDirection();
            _animationController = new Animations.AnimationController("Player");
            for(int i = 4; i < 8; i ++)
                _animationController.AnimationsList[i].OnAnimationEnd += Player_OnAnimationEnd;
            _lasDirection = Direction.DOWN;
            _velocity = new Vector2(80.0f, 80.0f);
            _directionVector = new Vector2(0, 0);

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

            MoveAndFixCollisionFraction(p_delta, p_collider, direction);

            _lasDirection = direction;

            base.Update(p_delta);
        }

        private Direction GetDefaultDirection()
        {
            var __dir = InputManager.GetDirection();
            return __dir.Item1 == Direction.NONE ? _lasDirection : __dir.Item1;
        }

        public void MoveAndFixCollisionFraction(float p_delta, Collider p_collider, Direction p_direction)
        {
            var __maxReach = 0.0f;

            var __tempPos = position + _directionVector * _velocity * p_delta;

            aabb.Min = __tempPos;
            aabb.Max = __tempPos + size;
            movementAABB.Min = __tempPos + movementAABBOffset;
            movementAABB.Max = __tempPos + movementAABBOffset + movementAABBSize;

            if (Math.Abs(_directionVector.X) > 0 || Math.Abs(_directionVector.Y) > 0)
            {
                _isColliding = p_collider.IsColliding(movementAABB);
            }

            if (_isColliding)
            {
                var __reachFraction = p_delta * 0.5f;

                for (var __i = 0; __i < 4; __i++)
                {
                    __tempPos = position + (_directionVector * _velocity * p_delta * __reachFraction);

                    _isColliding = p_collider.IsColliding(new AABB(__tempPos, __tempPos + size), p_direction);

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

        public bool IsHealthFull()
        {
            return _maximumLife == health;
        }
        public void SetAttackAnimation()
        {
            _animationController.ChangeAnimation("Attack" 
                + _animationController.Animation.name.Remove(0, 4));
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
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