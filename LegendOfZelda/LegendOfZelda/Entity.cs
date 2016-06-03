using Microsoft.Xna.Framework.Graphics;
namespace LegendOfZelda
{
    public class Entity
    {
        public Scene scene;
        public Entity() { }
        public virtual void Update(float delta) {}
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void DebugDraw(SpriteBatch spriteBatch) { }
    }
}
