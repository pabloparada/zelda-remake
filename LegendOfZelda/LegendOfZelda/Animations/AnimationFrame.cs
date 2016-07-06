using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendOfZelda.Items;

namespace LegendOfZelda.Animations
{
    public class AnimationFrame
    {
        public TilesetManager.TileSetType frameType;

        public TilesetManager.ItemTileSet itemTile;

        public float duration;

        public AnimationFrame(TilesetManager.ItemTileSet p_itemTile, float p_duration = 0.2f)
        {
            frameType = TilesetManager.TileSetType.ITEMS;
            itemTile = p_itemTile;
            duration = p_duration;
        }

        public int GetFrameIndex()
        {
            if (frameType == TilesetManager.TileSetType.ITEMS)
                return (int)itemTile;
            return 0;
        }
        
    }
}
