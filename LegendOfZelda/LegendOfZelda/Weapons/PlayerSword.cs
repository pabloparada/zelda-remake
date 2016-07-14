using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class PlayerSword : Weapon
    {
        private Vector2 _velocity;
        private Rectangle _hitbox;
        private AABB _aabb;
        private Vector2 _projectileSize;
        private Vector2 _playerSize;

        private bool _switchedComponents;
        private bool _isColliding;

        public PlayerSword(Entity p_source, Direction p_direction) : base(p_source, new Vector2(5.0f, 5.0f), p_direction)
        {
            _projectileSize = GetProjectileSizeAndControlComponentSwitch(new Vector2(12, 4));
            _playerSize = new Vector2(12, 12);
            _velocity = new Vector2(150, 150);

            _maxCooldown = 2.0f;

            position = CenterPositionByDirection(_playerSize, _projectileSize);
            
            _aabb = new AABB(p_source.position, p_source.position + _projectileSize);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            _cooldown += p_delta;

            if (state != State.ACTIVE) return;

            var __direction = InputManager.GetDirectionVectorByDirectionEnum(direction);

            position += __direction *_velocity * p_delta;

            _projectileSize = GetProjectileSizeAndControlComponentSwitch(_projectileSize);

            _aabb.Min = position;
            _aabb.Max = position + _projectileSize;

            _isColliding = p_collider.IsColliding(_aabb, direction);

            if (_isColliding)
            {
                state = State.DISABLED;
            }

            base.Update(p_delta, p_collider);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(_aabb.ScaledRectangleFromAABB(_projectileSize), Color.Red, 1.0f);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(_aabb.ScaledRectangleFromAABB(_projectileSize), Color.Blue);
        }
    }
}
