using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class SwordProjectile : Projectile
    {
        private Vector2 _velocity;
        private Rectangle _hitbox;
        private AABB _aabb;
        private Vector2 _projectileSize;
        private Vector2 _playerSize;

        private bool _switchedComponents;
        private bool _isColliding;

        public SwordProjectile(Vector2 p_position, Direction p_direction) : base(p_position, p_direction)
        {
            _projectileSize = GetProjectileSizeAndControlComponentSwitch(new Vector2(12, 4));
            _playerSize = new Vector2(12, 12);
            _velocity = new Vector2(180, 180);

            position = p_position = GetInitialPositionByDirection(_playerSize, _projectileSize);
            
            _aabb = new AABB(p_position, p_position + _projectileSize);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            var __direction = InputManager.GetDirectionVectorByDirectionEnum(direction);

            position += __direction *_velocity * p_delta;

            _projectileSize = GetProjectileSizeAndControlComponentSwitch(_projectileSize);

            _aabb.Min = position;
            _aabb.Max = position + _projectileSize;

            _isColliding = p_collider.IsColliding(_aabb, direction);

            if (_isColliding)
            {
                alive = false;
                _switchedComponents = false;
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

        private Vector2 Switch(Vector2 p_v1)
        {
            _switchedComponents = true;
            return new Vector2(p_v1.Y, p_v1.X);
        }

        private Vector2 GetProjectileSizeAndControlComponentSwitch(Vector2 p_projectileSize)
        {
            return !IsVerticalMovement() && !_switchedComponents ? Switch(p_projectileSize) : p_projectileSize;
        }
    }
}
