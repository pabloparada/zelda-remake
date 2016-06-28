using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class BoomerangProjectile : Projectile
    {
        private readonly AABB _aabb;
        private readonly Vector2 _projectileSize;
        private readonly Vector2 _playerSize;
        private readonly Vector2 _velocity;
        private readonly Vector2 _direction;
        private readonly Vector2 _reverseDirection;
        private readonly Vector2 _maxDistance;

        private Vector2 _maxDistanceInDirection;
        private Vector2 _currentDistance;
        private Vector2 _currentPlayerPosition;

        private bool _switchedDirection;

        private bool _isColliding;
        

        public BoomerangProjectile(Vector2 p_position, Direction p_direction) : base(p_position, p_direction)
        {
            _projectileSize = GetProjectileSizeAndControlComponentSwitch(new Vector2(3, 10));
            _playerSize = new Vector2(12, 12);
            _velocity = new Vector2(130, 130);
            position = _currentPlayerPosition = p_position = GetInitialPositionByDirection(_playerSize, _projectileSize);
            _aabb = new AABB(p_position, p_position + _projectileSize);
            _currentDistance = Vector2.Zero;
            _direction = InputManager.GetDirectionVectorByDirectionEnum(direction);
            _reverseDirection = RevertDirection(_direction);
            _maxDistance = new Vector2(55.0f, 55.0f);
            _maxDistanceInDirection = GetMaxDistanceInDirection();
            _switchedDirection = false;
        }

        public override void Update(float p_delta, Collider p_collider, Vector2 p_playerPosition)
        {
            _currentPlayerPosition = p_playerPosition;

            var __tmpPosition = position;
            var __direction = ShouldSwitchDirection() ? _reverseDirection : _direction;

            if (_switchedDirection)
            {
                position += GetInitialPositionByDirection(p_playerPosition - position, _playerSize, _projectileSize) * p_delta * 5.0f;
            }
            else
            {
                position += __direction * _velocity * p_delta * 1.2f;
            }

            _currentDistance += position - __tmpPosition;

            _aabb.Min = position;
            _aabb.Max = position + _projectileSize;

            if (IsBoomerangMovimentEnded(p_collider))
            {
                ResetBoomerangState();
            }

            base.Update(p_delta, p_collider, p_playerPosition);
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

        private void ResetBoomerangState()
        {
            alive = false;
            _switchedDirection = false;
            _currentDistance = Vector2.Zero;
            _maxDistanceInDirection = GetMaxDistanceInDirection();

        }

        private bool IsBoomerangMovimentEnded(Collider p_collider)
        {
            return p_collider.PointInsideRectangle(_aabb.Min, _currentPlayerPosition, _currentPlayerPosition + _playerSize) || 
                   p_collider.PointInsideRectangle(_aabb.Max, _currentPlayerPosition, _currentPlayerPosition + _playerSize);
        }

        private bool ShouldSwitchDirection()
        {
            var __isAtMax = false;

            if (direction == Direction.UP) __isAtMax = _currentDistance.Y <= _maxDistanceInDirection.Y;
            else if (direction == Direction.DOWN) __isAtMax = _currentDistance.Y >= _maxDistanceInDirection.Y;
            else if (direction == Direction.LEFT) __isAtMax = _currentDistance.X <= _maxDistanceInDirection.X;
            else if (direction == Direction.RIGHT) __isAtMax = _currentDistance.X >= _maxDistanceInDirection.X;

            if (__isAtMax)
            {
                _switchedDirection = true;
            }

            return _switchedDirection;
        }

        private Vector2 GetMaxDistanceInDirection()
        {
            return _maxDistance * _direction;
        }
    }
}
