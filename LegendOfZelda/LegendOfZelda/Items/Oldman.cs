using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Util;

namespace LegendOfZelda.Items
{
    public class Oldman : Item
    {
        public Oldman(Object p_obj)
        {
            tag = "Oldman";
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            hitboxSize = new Vector2(16f, 16f);
            hitboxOffset = new Vector2(0f, 0f);
            UpdateAABB();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(GraphicAssets.itemsTileset, MathUtil.GetDrawRectangle(position, size, parentPosition),
                TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, (int)TilesetManager.ItemTileSet.OLDMAN), Color.White);
        }
    }
}
