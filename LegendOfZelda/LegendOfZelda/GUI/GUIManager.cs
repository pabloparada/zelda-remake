using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda.GUI
{ 
    public class GUIManager : Entity
    {
        public GUIMap map;
        public GUIItems items;
        public GUIInventory inventory;

        public GUIManager(Player p_player)
        {
            position = new Vector2(0f, -176f);
            map = new GUIMap();
            items = new GUIItems(p_player);
            inventory = new GUIInventory();
            size = new Vector2(256.0f, 226.0f);
        }
        public void ForcePosition(Vector2 p_pos)
        {
            position = p_pos;
        }
        public override void Update(float p_delta)
        {
            base.Update(p_delta);
            map.parentPosition = position;
            map.Update(p_delta);
            items.parentPosition = position;
            items.Update(p_delta);
            inventory.parentPosition = position;
            inventory.Update(p_delta);
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);

            //Draw Black Fill Background
            p_spriteBatch.FillRectangle(MathUtil.GetDrawRectangle(position, size, parentPosition), Color.Black);

            map.Draw(p_spriteBatch);
            items.Draw(p_spriteBatch);
            inventory.Draw(p_spriteBatch);
        }
    }
}
