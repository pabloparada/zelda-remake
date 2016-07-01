using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        
        public Vector2 _position { get; private set; }
        private readonly Vector2 _velocity;
        private Vector2 _direction;

        private AABB _weaponAABB;
        public AABB _playerAABB;
        private Rectangle _hitbox;

        private bool _isColliding;
        private bool _switchSwordAttackDirection;

        private Projectile _projectile;

        private readonly Vector2 _linkSpriteSize;
        private Vector2 _swordSpriteSize;

        private readonly SpriteFont _font;

        private float _attackCooldown;

        private Direction _lasDirection;

        public Player(GraphicsDeviceManager p_graphicsDeviceManager)
        {
            state = State.ACTIVE;
            _position = new Vector2(190, 80);
            _velocity = new Vector2(80.0f, 80.0f);
            _direction = new Vector2(0, 0);
            _linkSpriteSize = new Vector2(12, 12);
            _swordSpriteSize = new Vector2(12, 4);
            _lasDirection = Direction.DOWN;
            _playerAABB = new AABB(_position, _position + _linkSpriteSize);
            _hitbox = new Rectangle(_position.ToPoint(), _linkSpriteSize.ToPoint());
            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }
        public void ForcePosition(Vector2 p_pos)
        {
            _position = p_pos;
            _hitbox.X = (int)_position.X;
            _hitbox.Y = (int)_position.Y;
        }
        public override void Update(float p_delta, Collider p_collider)
        {
            var __dirTuple = InputManager.GetDirection();
            var __dir = __dirTuple.Item1 == Direction.NONE ? _lasDirection : __dirTuple.Item1;
            _direction = __dirTuple.Item2;

            MoveAndFixCollisionFraction(p_delta, p_collider, __dir);
            Attack(p_delta, p_collider, __dir);

            _lasDirection = __dir;

            base.Update(p_delta);
        }

        public void MoveAndFixCollisionFraction(float p_delta, Collider p_collider, Direction p_direction)
        {
            var __maxReach = 0.0f;

            var __tempPos = _position + _direction * _velocity * p_delta;

            _playerAABB.Min = __tempPos;
            _playerAABB.Max = __tempPos + _linkSpriteSize;

            if (Math.Abs(_direction.X) > 0 || Math.Abs(_direction.Y) > 0)
            {
                _isColliding = p_collider.IsColliding(_playerAABB, p_direction);
            }

            if (_isColliding)
            {
                var __reachFraction = p_delta * 0.5f;

                for (var i = 0; i < 4; i++)
                {
                    __tempPos = _position + (_direction * _velocity * p_delta * __reachFraction);

                    _isColliding = p_collider.IsColliding(new AABB(__tempPos, __tempPos + _linkSpriteSize), p_direction);

                    if (_isColliding)
                        __reachFraction -= 1f / (float)Math.Pow(2, i + 2);
                    else
                    {
                        __reachFraction += 1f / (float)Math.Pow(2, i + 2);
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
                _hitbox.X = (int)_position.X;
                _hitbox.Y = (int)_position.Y;
            }
        }

        public void Attack(float p_delta, Collider p_collider, Direction p_direction)
        {
            // if projectile is not alive and cd aint up keep updating
            if (_projectile != null && (_projectile.alive || !_projectile.IsCooldownUp()))
            {
                _projectile.Update(p_delta, p_collider, _position);
            }
            // projectile is not alive and cd is up so let player throw projectiles again
            else
            {
                _projectile = null;
            }

            // player is pressing X or some projectile is alive
            if (InputManager.GetKeyChange(Keys.X) || _weaponAABB != null)
            {
                if (_projectile != null)
                {
                    if (_attackCooldown <= 0.5f)
                    {
                        _weaponAABB = new AABB(_projectile.GetInitialPositionByDirection(_position, _linkSpriteSize, _swordSpriteSize), _linkSpriteSize);
                        _attackCooldown += p_delta;
                    }
                    else
                    {
                        _weaponAABB = null;
                        _attackCooldown = 0.0f;
                    }
                }
                else
                {
                    _projectile = new SwordProjectile(_position, p_direction);
                    _weaponAABB = null;
                    _attackCooldown = 0.0f;
                }
            }

            // change player attack direction
            if (_lasDirection == p_direction)
            {
                if (!_switchSwordAttackDirection && (p_direction == Direction.UP || p_direction == Direction.DOWN))
                {
                    _switchSwordAttackDirection = true;
                    _swordSpriteSize = new Vector2(_swordSpriteSize.Y, _swordSpriteSize.X);
                }
                else if (_switchSwordAttackDirection && (p_direction == Direction.LEFT || p_direction == Direction.RIGHT))
                {
                    _switchSwordAttackDirection = false;
                    _swordSpriteSize = new Vector2(_swordSpriteSize.Y, _swordSpriteSize.X);
                }
            }
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            Rectangle rect = new Rectangle((int)((parentScenePosition.X+_position.X) * Main.s_scale), (int) ((parentScenePosition.Y + _position.Y) * Main.s_scale + 48 * Main.s_scale),  _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);
            p_spriteBatch.FillRectangle(rect, Color.Green);

            if (_projectile != null && _projectile.alive) _projectile.Draw(p_spriteBatch);
            if (_weaponAABB != null) p_spriteBatch.FillRectangle(_weaponAABB.ScaledRectangleFromAABB(_swordSpriteSize), Color.Aqua);

            base.DebugDraw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            Rectangle debugHitbox = new Rectangle((int)(parentScenePosition.X + _hitbox.X) * Main.s_scale, (int)(parentScenePosition.Y + _hitbox.Y) * Main.s_scale + 48 * Main.s_scale,  _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);

            p_spriteBatch.DrawRectangle(debugHitbox, Color.ForestGreen, 1.0f);

            if (_isColliding) p_spriteBatch.DrawRectangle(debugHitbox, Color.Red, 1.0f);

            if (_projectile != null && _projectile.alive) _projectile.DebugDraw(p_spriteBatch);
            if (_weaponAABB != null) p_spriteBatch.DrawRectangle(_weaponAABB.ScaledRectangleFromAABB(_swordSpriteSize), Color.DarkRed);

            var __msgPos = new Vector2((parentScenePosition.X + _position.X) * Main.s_scale, (parentScenePosition.Y + _position.Y) * Main.s_scale + 48 * Main.s_scale);

            p_spriteBatch.DrawString(_font, "X:" + (int) _position.X + " Y:" + (int) _position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
