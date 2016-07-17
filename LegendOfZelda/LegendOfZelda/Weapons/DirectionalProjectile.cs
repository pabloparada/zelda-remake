using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class DirectionalProjectile : Weapon
    {
        private readonly Vector2 _velocity;

        private Vector2 _projectileSize;

        public DirectionalProjectile(Entity p_source) : base(p_source, new Vector2(3.6f, 15.0f), p_source.direction)
        {
            _projectileSize = GetProjectileSizeAndControlComponentSwitch(size);
            _velocity = new Vector2(150, 150);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            cooldown += p_delta;

            if (state != State.ACTIVE) return;

            var __direction = InputManager.GetDirectionVectorByDirectionEnum(direction);

            position += __direction * _velocity * p_delta;

            _projectileSize = GetProjectileSizeAndControlComponentSwitch(_projectileSize);

            if (!IsAtScreenBoundaries(position, size))
            {
                state = State.DISABLED;
            }

            base.Update(p_delta, p_collider);
        }

        public override void OnCollide(Entity p_entity)
        {
            if (p_entity.type != EntityType.ENEMY) return;

            state = State.DISABLED;

            base.OnCollide(p_entity);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition), Color.Blue);
        }
    }
}