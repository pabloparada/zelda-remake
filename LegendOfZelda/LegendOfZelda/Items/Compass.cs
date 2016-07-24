using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class Compass : Item
    {
        public Compass(Object p_obj)
        {
            tag = "Compass";
            _animationController = new Animations.AnimationController("Compass");
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
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
            _animationController.DrawFrame(spriteBatch,
                MathUtil.GetDrawRectangle(position, size, parentPosition));
        }
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
            if (p_entity.type == EntityType.PLAYER)
            {
                Inventory.Instance.hasCompass = true;
                DestroyEntity();
            }
        }
    }
}
