using System.Diagnostics;
using LegendOfZelda.Animations;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class DirectionalProjectile : Weapon
    {
        private readonly Vector2 _velocity;

        private Vector2 _projectileSize;

        public DirectionalProjectile(Entity p_source, Vector2 p_size, Vector2 p_aabbOffset, string p_animationName) : base(p_source, p_size, p_source.direction)
        {
            weaponType = WeaponType.PROJECTILE;
            hitboxOffset = p_aabbOffset;

            _projectileSize = GetProjectileSizeAndControlComponentSwitch(size);
            _velocity = new Vector2(180.0f, 180.0f);

            _animationController = new AnimationController(p_animationName);
            _animationController.ChangeAnimation(InputManager.GetAnimationNameByDirection(p_source.direction));

            animationSpeed = 4.0f;
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
            _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition));
        }
    }
}