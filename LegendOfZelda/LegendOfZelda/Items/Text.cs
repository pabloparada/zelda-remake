using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Util;


namespace LegendOfZelda.Items
{

    public class Text : Item
    {
        private string text1;
        private string text2;
        public Text(Object p_obj)
        {
            tag = "Text";
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            hitboxSize = new Vector2(16f, 16f);
            hitboxOffset = new Vector2(0f, 0f);
            text1 = p_obj.properties.Text1;
            text2 = p_obj.properties.Text2;
            UpdateAABB();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(GraphicAssets.zeldaFont12, text1, 
                (position + (Vector2.UnitY * 2f) + parentPosition) * Main.s_scale, 
                Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(GraphicAssets.zeldaFont12, text2, 
                (position + (Vector2.UnitY * 18f) + parentPosition) * Main.s_scale,
                Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }
    }
}
