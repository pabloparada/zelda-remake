using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        
        public Vector2 Position { get; private set; }
        private Vector2 _velocity;
        private Vector2 _direction;

        private AABB _swordAABB;
        public AABB _playerAABB;
        private Rectangle _hitbox;

        private bool _isColliding;

        private Projectile _projectile;

        private Vector2 _linkSpriteSize;
        private Vector2 _swordSpriteSize;

        private SpriteFont _font;

        private float _attackCooldown;

        private Direction _lasDirection;

        public Player(GraphicsDeviceManager p_graphicsDeviceManager)
        {
            state = State.ACTIVE;
            //Position = new Vector2(66, 40);
            Position = new Vector2(150, 80);
            _velocity = new Vector2(160.0f, 160.0f);
           // _velocity = new Vector2(80.0f, 80.0f);
            _direction = new Vector2(0, 0);
            _linkSpriteSize = new Vector2(12, 12);
            _swordSpriteSize = new Vector2(12, 4);
            _lasDirection = Direction.DOWN;
            _playerAABB = new AABB(Position, Position + _linkSpriteSize);
            _hitbox = new Rectangle(Position.ToPoint(), _linkSpriteSize.ToPoint());
            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }
        public void ForcePosition(Vector2 p_pos)
        {
            Position = p_pos;
            _hitbox.X = (int)Position.X;
            _hitbox.Y = (int)Position.Y;
        }
        public override void Update(float p_delta, Collider p_collider)
        {
            var __dirTuple = InputManager.GetDirection();

            var __dir = __dirTuple.Item1 == Direction.NONE ? _lasDirection : __dirTuple.Item1;

            var __maxReach = 0.0f;

            _direction = __dirTuple.Item2;

            var __tempPos = Position + _direction * _velocity * p_delta;

            _playerAABB.Min = __tempPos;
            _playerAABB.Max = __tempPos + _linkSpriteSize;

            if (Math.Abs(_direction.X) > 0 || Math.Abs(_direction.Y) > 0)
            {
                _isColliding = p_collider.IsColliding(_playerAABB, __dir);
            }
            
            if (_isColliding)
            {
                var __reachFraction = p_delta * 0.5f;

                for (var i = 0; i < 4; i++)
                {
                    __tempPos = Position + (_direction*_velocity*p_delta*__reachFraction);

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
                Position += _direction * _velocity * p_delta * __maxReach;
                Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
                _hitbox.X =  (int)Position.X;
                _hitbox.Y = (int)Position.Y;
            }

            if (_projectile != null && _projectile.alive)
            {
                _projectile.Update(p_delta, p_collider, Position);
            }
            else
            {
                _projectile = null;
            }

            if (InputManager.GetKeyChange(Keys.X) || _swordAABB != null)
            {
                if (_projectile != null)
                {
                    if (_attackCooldown <= 0.5f)
                    {
                        _swordAABB = new AABB(_projectile.GetInitialPositionByDirection(Position, _linkSpriteSize, _swordSpriteSize), _linkSpriteSize);
                        _attackCooldown += p_delta;
                    }
                    else
                    {
                        _swordAABB = null;
                        _attackCooldown = 0.0f;
                    }
                }
                else
                {
                    _projectile = new SwordProjectile(Position, __dir);
                    _swordAABB = null;
                    _attackCooldown = 0.0f;
                }
            }

            _lasDirection = __dir;

            base.Update(p_delta);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            Rectangle rect = new Rectangle((int)((parentScenePosition.X+Position.X) * Main.s_scale), (int) ((parentScenePosition.Y + Position.Y) * Main.s_scale + 48 * Main.s_scale),  _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);
            p_spriteBatch.FillRectangle(rect, Color.Green);

            if (_projectile != null && _projectile.alive) _projectile.Draw(p_spriteBatch);
            if (_swordAABB != null) p_spriteBatch.FillRectangle(_swordAABB.ScaledRectangleFromAABB(_swordSpriteSize), Color.Aqua);

            base.DebugDraw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            Rectangle debugHitbox = new Rectangle((int)(parentScenePosition.X + _hitbox.X) * Main.s_scale, (int)(parentScenePosition.Y + _hitbox.Y) * Main.s_scale + 48 * Main.s_scale,  _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);

            p_spriteBatch.DrawRectangle(debugHitbox, Color.ForestGreen, 1.0f);

            if (_isColliding) p_spriteBatch.DrawRectangle(debugHitbox, Color.Red, 1.0f);

            if (_projectile != null && _projectile.alive) _projectile.DebugDraw(p_spriteBatch);
            if (_swordAABB != null) p_spriteBatch.DrawRectangle(_swordAABB.ScaledRectangleFromAABB(_swordSpriteSize), Color.DarkRed);

            var __msgPos = new Vector2((parentScenePosition.X + Position.X) * Main.s_scale, (parentScenePosition.Y + Position.Y) * Main.s_scale + 48 * Main.s_scale);

            p_spriteBatch.DrawString(_font, "X:" + (int) Position.X + " Y:" + (int) Position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
