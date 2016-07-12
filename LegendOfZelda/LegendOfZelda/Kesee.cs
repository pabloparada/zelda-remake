using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Kesee : Enemy
    {
        private readonly Vector2 _velocity;

        private Vector2 _targetDirection;
        private Vector2 _targetPosition;

        private float _animationTick;
        private readonly float _animationSpeed;
        private float _interpolatedTime;
        private float _sleepTime;

        public Kesee(Vector2 p_position) : base(p_position, new Vector2(15.0f, 15.0f))
        {
            _animationTick = 0.0f;
            _animationSpeed = 2.5f;
            _animationController = new AnimationController("Kesee");

            _velocity = new Vector2(35.0f, 35.0f);
            _targetPosition = position;
            _sleepTime = (float) (World.random.NextDouble() * World.random.Next(1, 6));
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (_sleepTime <= 0.0f)
            {
                if (_animationTick >= 2.0f) _animationTick = 0.0f;

                _animationTick += p_delta * 0.1f;

                _interpolatedTime = InOutQuad(_animationTick);

                var __tmpPosition = position + (_velocity*_interpolatedTime)*_targetDirection*p_delta;

                if (ReachedTargetPosition(__tmpPosition, _targetPosition) || IsColliding(__tmpPosition))
                {
                    SortNextMove();
                }
                else
                {
                    position = __tmpPosition;
                }
            }
            else
            {
                _sleepTime -= p_delta;
            }

            base.Update(p_delta);
        }

        private bool IsColliding(Vector2 p_targetPos)
        {
            return IsBoundary(p_targetPos) || IsBoundary(p_targetPos + size);
        }

        private static float InOutQuad(float p_time)
        {
            if (p_time <= 0.5f)
            {
                var __in = 2.0f * (p_time * p_time);

                return MathHelper.Clamp(__in, 0.0001f, 1.0f);
            }

            p_time -= 0.5f;

            var __out = 2.0f * p_time * (1.0f - p_time) + 0.5f;

            return MathHelper.Clamp(__out, 0.0001f, 1.0f);
        }

        private void SortNextMove()
        {
            var __x = World.random.Next(-1, 2);
            var __y = World.random.Next(-1, 2);

            _targetDirection = new Vector2(__x, __y);

            if (__x != 0 && __y != 0) {
                _targetDirection.Normalize();
            }

            var __targetDistance = 16.0f * World.random.Next(1, 5);

            _targetPosition = position + (_targetDirection * __targetDistance);
        }

        private static bool ReachedTargetPosition(Vector2 p_pos, Vector2 p_targetPos)
        {
            var __dist = Vector2.Distance(p_pos, p_targetPos);

            return __dist >= -0.5f && __dist <= 0.5f;
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            animationSpeed = _animationSpeed * _interpolatedTime;

            _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(position, size, parentPosition), Color.Bisque);
            base.DebugDraw(p_spriteBatch);
        }
    }
}
