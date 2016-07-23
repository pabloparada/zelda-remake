using LegendOfZelda.Util;
using LegendOfZelda.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class DeathExplosion : Entity
    {
        public DeathExplosion(Vector2 p_pos)
        {
            type = EntityType.EFFECTS;
            tag = "SpawnExplosion";
            position = new Vector2(p_pos.X, p_pos.Y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            _animationController = new AnimationController("DeathExplosion");
            _animationController.Animation.OnAnimationEnd += DestroyEntity;
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
        }

        private void Animation_OnAnimationEnd()
        {
            DestroyEntity();
        }

        public override void Update(float delta, Collider p_collider)
        {
            base.Update(delta, p_collider);
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
            if (p_entity.type == EntityType.PLAYER)
                DestroyEntity();
        }
    }
}
