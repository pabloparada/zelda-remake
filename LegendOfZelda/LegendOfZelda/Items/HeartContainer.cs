using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class HeartContainer : Item
    {
        public HeartContainer(Object p_obj)
        {
            tag = "HeartContainer";
            _animationController = new Animations.AnimationController("HeartContainer");
            spawn = (SpawnType)p_obj.properties.KeyType;
            if (spawn == SpawnType.ALWAYS)
                state = State.ACTIVE;
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            hitboxSize = new Vector2(12f, 12f);
            hitboxOffset = new Vector2(2f, 2f);
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
                DestroyEntity();
        }
        public override void AllDead()
        {
            base.AllDead();
            if (spawn == SpawnType.ALL_DEAD)
                state = State.ACTIVE;
        }
    }
}
