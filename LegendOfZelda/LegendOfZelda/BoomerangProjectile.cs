using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class BoomerangProjectile : Projectile
    {
        private AABB _aabb;

        private Vector2 _projectileSize;
        private Vector2 _playerSize;
        private Vector2 _velocity;
        private Vector2 _direction;
        private Vector2 _currentPlayerPosition;
        
        private bool _paused;

        private bool _isColliding;
        

        public BoomerangProjectile(Vector2 p_position, Direction p_direction) : base(p_position, p_direction)
        {
            _projectileSize = GetProjectileSizeAndControlComponentSwitch(new Vector2(3, 10));
            _playerSize = new Vector2(12, 12);
            _velocity = new Vector2(130, 130);
            position = _currentPlayerPosition = p_position = GetInitialPositionByDirection(_playerSize, _projectileSize);
            _aabb = new AABB(p_position, p_position + _projectileSize);
        }

        public override void Update(float p_delta, Collider p_collider, Vector2 p_playerPosition)
        {
            if (InputManager.GetKeyChange(Keys.Space)) _paused = !_paused;

            _currentPlayerPosition = p_playerPosition;

            if (!_paused)
            {
                _direction = InputManager.GetDirectionVectorByDirectionEnum(direction);

                position += _direction * _velocity* p_delta;

                _projectileSize = GetProjectileSizeAndControlComponentSwitch(_projectileSize);

                _aabb.Min = position;
                _aabb.Max = position + _projectileSize;

                _isColliding = p_collider.IsColliding(_aabb, direction);

                if (_isColliding)
                {
                    alive = false;
                }

                base.Update(p_delta, p_collider, p_playerPosition);
            }
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {

            p_spriteBatch.FillRectangle(_aabb.ScaledRectangleFromAABB(_projectileSize), Color.Blue);
            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            var __thickness = 3.0f;

            var __scaledDebugInitialPlayerPosition = ScaleVectorForDrawing(initialPlayerPosition);
            var __scaledDebugBoomerangPosition = ScaleVectorForDrawing(position);

            p_spriteBatch.DrawLine(__scaledDebugInitialPlayerPosition, __scaledDebugBoomerangPosition, Color.Red, __thickness);

            if (_currentPlayerPosition != initialPlayerPosition)
            {
                var __scaledDebugPlayerPosition = ScaleVectorForDrawing(_currentPlayerPosition);

                p_spriteBatch.DrawLine(__scaledDebugPlayerPosition, __scaledDebugBoomerangPosition, Color.Green, __thickness);
                p_spriteBatch.DrawLine(__scaledDebugPlayerPosition, __scaledDebugInitialPlayerPosition, Color.Blue, __thickness);
            }

            
            p_spriteBatch.DrawRectangle(_aabb.ScaledRectangleFromAABB(_projectileSize), Color.Red);

            base.DebugDraw(p_spriteBatch);
        }

        private Vector2 ScaleVectorForDrawing(Vector2 p_vector)
        {
            return new Vector2(p_vector.X * Main.s_scale, p_vector.Y * Main.s_scale + 48 * Main.s_scale);
        }
    }


}
