﻿using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class WoodSwordItem : Item
    {
        public WoodSwordItem(Object p_obj)
        {
            tag = "WoodSword";
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
            spriteBatch.Draw(GraphicAssets.itemsTileset, 
                            MathUtil.GetDrawRectangle(position, size, parentPosition),
                            TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, 
                            (int)TilesetManager.ItemTileSet.WOOD_SWORD), 
                            Color.White);
        }
    }
}
