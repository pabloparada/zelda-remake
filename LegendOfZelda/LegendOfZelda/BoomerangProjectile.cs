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
        private Vector2 _reverseDirection;
        private Vector2 _currentPlayerPosition;
        private Vector2 _maxDistanceInDirection;
        private Vector2 _currentDistance;
        private Vector2 _maxDistance;
        private Vector2 _t;
        private Vector2 _playerDeltaPositon;

        private bool _paused;
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
            _t = Vector2.Zero;
            _playerDeltaPositon = Vector2.Zero;
            _maxDistance = new Vector2(60.0f, 60.0f);
            _maxDistanceInDirection = GetMaxDistanceInDirection();
            _switchedDirection = false;
        }

        public override void Update(float p_delta, Collider p_collider, Vector2 p_playerPosition)
        {
            if (InputManager.GetKeyChange(Keys.Space)) _paused = !_paused;

            _currentPlayerPosition = p_playerPosition;

            if (!_paused)
            {
                var __tmpPosition = position;
                var __direction = ShouldSwitchDirection() ? _reverseDirection : _direction;

                _currentDistance += position - __tmpPosition;

                _t = GetInterpolatedTimeByDistance();

                if (_switchedDirection)
                {
                    _playerDeltaPositon = initialPlayerPosition - _currentPlayerPosition;
                    
                    if (!IsPlayerMovingLeftOrRight() && IsProjectileMovingLeftOrRight())
                    {
                        if (_playerDeltaPositon.X == 0.0f)
                        {
                            _playerDeltaPositon.Y += _t.Y * __direction.X;
                        }
                        else
                        {
                            _playerDeltaPositon.X += _t.X * __direction.Y;
                        }

                        System.Console.WriteLine(_playerDeltaPositon);
                    }
                }
                else
                {
                    position += __direction * _velocity * p_delta;
                }

                _aabb.Min = position;
                _aabb.Max = position + _projectileSize;

                _isColliding = p_collider.IsColliding(_aabb, direction);

                if (_isColliding || IsBoomerangMovimentEnded())
                {
                    ResetBoomerangState();
                }

                base.Update(p_delta, p_collider, p_playerPosition);
            }
        }

        private Vector2 GetInterpolatedTimeByDistance()
        {
            var __t = _currentDistance / _maxDistance;

            if (__t.X >= 1.0f) __t.X = 1.0f;
            else if (__t.X <= -1.0f) __t.X = -1.0f;
            else if (__t.Y >= 1.0f) __t.Y = 1.0f;
            else if (__t.Y <= -1.0f) __t.Y = -1.0f;

            if (__t.X == 0.0f) __t.X = __t.Y;
            if (__t.Y == 0.0f) __t.Y = __t.X;

            return __t;
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

        private bool IsBoomerangMovimentEnded()
        {
            return _currentDistance.X >= -0.2f && _currentDistance.X <= 0.2f && _currentDistance.Y >= -0.2f && _currentDistance.Y <= 0.2f && _switchedDirection;
        }

        private bool ShouldSwitchDirection()
        {
            var isAtMax = false;

            if (direction == Direction.UP) isAtMax = _currentDistance.Y <= _maxDistanceInDirection.Y;
            else if (direction == Direction.DOWN) isAtMax = _currentDistance.Y >= _maxDistanceInDirection.Y;
            else if (direction == Direction.LEFT) isAtMax = _currentDistance.X <= _maxDistanceInDirection.X;
            else if (direction == Direction.RIGHT) isAtMax = _currentDistance.X >= _maxDistanceInDirection.X;

            if (isAtMax)
            {
                _switchedDirection = isAtMax;
                return _switchedDirection;
            }

            return _switchedDirection;
        }

        private Vector2 GetMaxDistanceInDirection()
        {
            return _maxDistance * _direction;
        }

        private bool IsPlayerMovingUpOrDown()
        {
            return _playerDeltaPositon.Y != 0.0f;
        }

        private bool IsPlayerMovingLeftOrRight()
        {
            return _playerDeltaPositon.X != 0.0f;
        }

        private bool IsProjectileMovingLeftOrRight()
        {
            return direction == Direction.LEFT || direction == Direction.RIGHT;
        }

        private bool IsProjectileMovingUpOrDown()
        {
            return direction == Direction.DOWN || direction == Direction.UP;
        }
    }


}
