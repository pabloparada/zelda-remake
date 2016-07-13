﻿using LegendOfZelda.Util;
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
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);

            p_spriteBatch.Draw(GraphicAssets.itemsTileset, 
                               MathUtil.GetDrawRectangle(position,size,parentPosition), 
                               TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, 
                               (int)TilesetManager.ItemTileSet.BOOMERANG), 
                               Color.White);
        }
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
            if (p_entity.type == EntityType.PLAYER)
                DestroyEntity();
        }
    }
}
