using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class WoodSwordItem : Item
    {
        public WoodSwordItem(Object p_obj)
        {
            tag = "WoodSword";
            _animationController = new Animations.AnimationController("WoodSword");
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
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
                Inventory.Instance.hasSword = true;
                DestroyEntity();
            }
        }
    }
}
