using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Entity
    {
        public enum State
        {
            ACTIVE,
            DRAW_ONLY,
            DISABLED
        }
        public State        state = State.DISABLED;
        public Vector2      parentScenePosition;
        public Vector2      position { get; protected set; }
        public Vector2      size { get; protected set; }

        public string       tag = "Entity";
        public string       name = "Entity";
        public virtual void Update(float delta) { }
        public virtual void Update(float delta, Collider p_collider) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void DebugDraw(SpriteBatch spriteBatch) { }
    }
}
