using System;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        private readonly Vector2 _velocity;
        private Vector2 _directionVector;

        private bool _isColliding;

        private readonly SpriteFont _font;

        private Direction _lasDirection;

        public Player()
        {
            type = EntityType.PLAYER;
            tag = "Player";
            name = "Player";
            state = State.ACTIVE;
            position = new Vector2(150, 80);
            size = new Vector2(12, 12);
            direction = GetDefaultDirection();
            _lasDirection = Direction.DOWN;
            _velocity = new Vector2(80.0f, 80.0f);
            _directionVector = new Vector2(0, 0);
            aabb = new AABB(position, position + size);
            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }
        public void ForcePosition(Vector2 p_pos)
        {
            position = p_pos;
        }
        public override void Update(float p_delta, Collider p_collider)
        {
            direction = GetDefaultDirection();

            _directionVector = InputManager.GetDirection().Item2;

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

            if (Math.Abs(_directionVector.X) > 0 || Math.Abs(_directionVector.Y) > 0)
            {
                _isColliding = p_collider.IsColliding(aabb, p_direction);
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

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(MathUtil.GetDrawRectangle(position, size, parentPosition), Color.Green);

            base.Draw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            var __debugHitbox = MathUtil.GetDrawRectangle(position, size, parentPosition);

            p_spriteBatch.DrawRectangle(__debugHitbox, Color.Green);

            if (_isColliding) p_spriteBatch.DrawRectangle(__debugHitbox, Color.Red, 1.0f);

            var __msgPos = new Vector2((parentPosition.X + position.X) * Main.s_scale, (parentPosition.Y + position.Y) * Main.s_scale);

            p_spriteBatch.DrawString(_font, "X:" + (int) position.X + " Y:" + (int) position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}