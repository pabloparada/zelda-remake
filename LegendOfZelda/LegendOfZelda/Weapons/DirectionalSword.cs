using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class DirectionalSword : Weapon
    {
        private readonly Vector2 _velocity;

        private Vector2 _projectileSize;
        private float _waitUntilDisabled;

        public DirectionalSword(Entity p_source) : base(p_source, new Vector2(3.6f, 15.0f), p_source.direction)
        {
            state = State.ACTIVE;

            _projectileSize = GetProjectileSizeAndControlComponentSwitch(size);
            _velocity = new Vector2(150, 150);

            _waitUntilDisabled = 1.0f;

            position = CenterPositionByDirection(p_source.position, p_source.size, _projectileSize);

            aabb = new AABB(p_source.position, p_source.position + _projectileSize);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            cooldown += p_delta;

            if (state != State.ACTIVE) return;

            var __direction = InputManager.GetDirectionVectorByDirectionEnum(direction);

            position += __direction * _velocity * p_delta;

            _projectileSize = GetProjectileSizeAndControlComponentSwitch(_projectileSize);

            if (!IsAtScreenBoundaries(position, size) && _waitUntilDisabled <= 0.0f)
            {
                state = State.DISABLED;
            }
            else
            {
                _waitUntilDisabled -= p_delta;
            }

            base.Update(p_delta, p_collider);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition), Color.Red, 1.0f);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition), Color.Blue);
        }
    }
}