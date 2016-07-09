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
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);

            p_spriteBatch.Draw(TilesetManager.itemsTileset, 
                               MathUtil.GetDrawRectangle(position,size,parentPosition), 
                               TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, 
                               (int)TilesetManager.ItemTileSet.BOOMERANG), 
                               Color.White);
        }
    }
}
