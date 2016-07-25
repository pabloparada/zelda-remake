using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class Key : Item
    {
        public Key(Object p_obj)
        {
            tag = "Key";
            _animationController = new Animations.AnimationController("Key");
            spawn = (SpawnType)p_obj.properties.KeyType;
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            if (spawn == SpawnType.ALWAYS)
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
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            base.DebugDraw(p_spriteBatch);
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(hitbox, parentPosition), Color.Yellow, 3f);
        }
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
            if (p_entity.type == EntityType.PLAYER)
            {
                DestroyEntity();
                Inventory.Instance.keyCount++;
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
