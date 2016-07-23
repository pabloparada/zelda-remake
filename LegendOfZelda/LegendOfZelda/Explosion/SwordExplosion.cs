using LegendOfZelda.Util;
using LegendOfZelda.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class SwordExplosion : Entity
    {
        private float           lifeSpan = 1f;
        private Vector2         _direction;

        public SwordExplosion(Vector2 p_pos, Vector2 p_direction)
        {
            type = EntityType.EFFECTS;
            tag = "SwordExplosion";
            position = new Vector2(p_pos.X, p_pos.Y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;

            _direction = p_direction;
            if (p_direction == new Vector2(-1f,1f))
                _animationController = new AnimationController("SwordExplosionUL");
            else if (p_direction == new Vector2(1f, 1f))
                _animationController = new AnimationController("SwordExplosionUR");
            else if (p_direction == new Vector2(-1f, -1f))
                _animationController = new AnimationController("SwordExplosionDL");
            else if (p_direction == new Vector2(1f, -1f))
                _animationController = new AnimationController("SwordExplosionDR");

            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
        }

        public override void Update(float delta, Collider p_collider)
        {
            base.Update(delta, p_collider);
            lifeSpan -= delta;
            position += new Vector2(_direction.X, _direction.Y * -1.2f) * delta * 16f;
            if (lifeSpan < 0f)
                DestroyEntity();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _animationController.DrawFrame(spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            base.DebugDraw(p_spriteBatch);
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(hitbox, parentPosition), Color.Yellow, 3f);
        }
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
        }
    }
}
