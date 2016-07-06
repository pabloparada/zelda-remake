using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class HeartContainer : Item
    {
        public HeartContainer(Object p_obj)
        {
            tag = "HeartContainer";
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
        }
        public override void Update(float delta, Collider p_collider)
        {
            base.Update(delta, p_collider);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(TilesetManager.itemsTileset, MathUtil.GetDrawRectangle(position, size, parentPosition),
                TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, (int)TilesetManager.ItemTileSet.HEART_CONTAINER), Color.White);
        }
    }
}
