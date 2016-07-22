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
            spriteBatch.Draw(GraphicAssets.itemsTileset, MathUtil.GetDrawRectangle(position, size, parentPosition),
                TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, (int)TilesetManager.ItemTileSet.KEY), Color.White);
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
    }
}
