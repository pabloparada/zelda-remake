using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Animations;
using LegendOfZelda.Util;

namespace LegendOfZelda.Items
{
    public class Heart : Item
    {
        public Heart(Object p_obj)
        {
            tag = "Heart";
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            _animationController = new AnimationController("Heart");
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
        }
        public Heart(Vector2 p_pos)
        {
            tag = "Heart";
            position = p_pos;
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            _animationController = new AnimationController("Heart");
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
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
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
            if (p_entity.type == EntityType.PLAYER)
                DestroyEntity();
        }
    }
}
