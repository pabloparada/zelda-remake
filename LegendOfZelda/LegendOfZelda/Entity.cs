using Microsoft.Xna.Framework.Graphics;
namespace LegendOfZelda
{
    public class Entity
    {
        public virtual void Update(float delta) { }
        public virtual void Update(float delta, Collider p_collider) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void DebugDraw(SpriteBatch spriteBatch) { }
    }
}
