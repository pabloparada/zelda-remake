using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class BoomerangItem : Item
    {
        public BoomerangItem(Object p_obj)
        {
            tag = "BoomerangItem";
            _animationController = new Animations.AnimationController("BoomerangItem");
            spawn = (SpawnType)p_obj.properties.KeyType;
            if (spawn == SpawnType.ALWAYS)
                state = State.ACTIVE;
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);
            _animationController.DrawFrame(p_spriteBatch, 
                MathUtil.GetDrawRectangle(position, size, parentPosition));
        }
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
            if (p_entity.type == EntityType.PLAYER)
            {
                Inventory.Instance.hasBoomerang = true;
                DestroyEntity();
            }
        }
        public override void AllDead()
        {
            base.AllDead();
            if (spawn == SpawnType.ALL_DEAD)
                state = State.ACTIVE;
        }
    }
}
