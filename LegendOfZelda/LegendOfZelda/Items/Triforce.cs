using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Items
{
    public class Triforce : Item
    {
        public TilesetManager.ItemTileSet itemTile = TilesetManager.ItemTileSet.TRIFORCE1;
        public float animationCount = 0f;

        public Triforce(Object p_obj)
        {
            tag = "Item";
            name = "Triforce";
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
        }
        public override void Update(float delta, Collider p_collider)
        {
            base.Update(delta, p_collider);

            animationCount += delta;
            if (animationCount >= 0.25f)
            {
                if (itemTile == TilesetManager.ItemTileSet.TRIFORCE1)
                    itemTile = TilesetManager.ItemTileSet.TRIFORCE2;
                else
                    itemTile = TilesetManager.ItemTileSet.TRIFORCE1;
                animationCount -= 0.25f;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Rectangle __rect = new Rectangle((int)position.X * Main.s_scale, ((int)position.Y * Main.s_scale) + 48 * Main.s_scale,
                (int)size.X * Main.s_scale, (int)size.X * Main.s_scale);
            spriteBatch.Draw(TilesetManager.itemsTileset, __rect, TilesetManager.GetSourceRectangle(TilesetManager.TileSetType.ITEMS, (int)itemTile), Color.White);
        }
    }
}
