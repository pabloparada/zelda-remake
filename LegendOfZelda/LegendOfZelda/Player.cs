using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _direction;

        private AABB _aabb;
        private Rectangle _hitbox;

        private bool _isColliding;

        private Projectile _projectile;

        private Vector2 _linkSpriteSize;

        private SpriteFont _font;

        private Direction _lasDirection;

        public Player(GraphicsDeviceManager p_graphicsDeviceManager)
        {
            _position = new Vector2(148, 100);
            _velocity = new Vector2(80.0f, 80.0f);
            _direction = new Vector2(0, 0);
            _linkSpriteSize = new Vector2(12, 12);
            _lasDirection = Direction.DOWN;

            _aabb = new AABB(_position, _position + _linkSpriteSize);
            _hitbox = new Rectangle(_position.ToPoint(), _linkSpriteSize.ToPoint());

            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            var __dirTuple = InputManager.GetDirection();

            var __dir = __dirTuple.Item1 == Direction.NONE ? _lasDirection : __dirTuple.Item1;

            var __maxReach = 0.0f;

            _direction = __dirTuple.Item2;

            var __tempPos = _position + _direction * _velocity * p_delta;

            _aabb.Min = __tempPos;
            _aabb.Max = __tempPos + _linkSpriteSize;

            if (Math.Abs(_direction.X) > 0 || Math.Abs(_direction.Y) > 0)
            {
                _isColliding = p_collider.IsColliding(_aabb, __dir);
            }
            
            if (_isColliding)
            {
                var __reachFraction = p_delta * 0.5f;

                for (var i = 0; i < 4; i++)
                {
                    __tempPos = _position + (_direction*_velocity*p_delta*__reachFraction);

                    _isColliding = p_collider.IsColliding(new AABB(__tempPos, __tempPos + _linkSpriteSize), __dir);

                    if (_isColliding)
                        __reachFraction -= 1f/(float) Math.Pow(2, i + 2);
                    else
                    {
                        __reachFraction += 1f/(float) Math.Pow(2, i + 2);
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
                _position += _direction * _velocity * p_delta * __maxReach;
                _position = new Vector2((float)Math.Round(_position.X), (float)Math.Round(_position.Y));
                _hitbox.X =  (int)_position.X;
                _hitbox.Y = (int)_position.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X) && _projectile == null)
            {
                _projectile = new SwordProjectile(_position, __dir);
            }

            if (_projectile != null && _projectile.alive)
            {
                _projectile.Update(p_delta, p_collider);
            }
            else
            {
                _projectile = null;
            }

            _lasDirection = __dir;

            base.Update(p_delta);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            var rect = new Rectangle((int) (_position.X * Main.s_scale), (int) (_position.Y * Main.s_scale + 48 * Main.s_scale), _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);
            p_spriteBatch.FillRectangle(rect, Color.Green);

            if (_projectile != null && _projectile.alive) _projectile.Draw(p_spriteBatch);

            base.DebugDraw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            var debugHitbox = new Rectangle(_hitbox.X * Main.s_scale, _hitbox.Y * Main.s_scale + 48 * Main.s_scale, _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);
            p_spriteBatch.DrawRectangle(debugHitbox, Color.ForestGreen, 1.0f);

            if (_isColliding)
                p_spriteBatch.DrawRectangle(debugHitbox, Color.Red, 1.0f);

            if (_projectile != null && _projectile.alive) _projectile.DebugDraw(p_spriteBatch);

            var __msgPos = new Vector2(_position.X * Main.s_scale, _position.Y * Main.s_scale + 48 * Main.s_scale);
            p_spriteBatch.DrawString(_font, "X:" + (int) _position.X + " Y:" + (int) _position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
