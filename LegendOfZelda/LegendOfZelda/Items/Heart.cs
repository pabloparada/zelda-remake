using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Animations;

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
            animationController = new AnimationController("Fire");
        }
        public override void Update(float delta, Collider p_collider)
        {
            base.Update(delta, p_collider);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            animationController.DrawFrame(spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
        }
    }
}
