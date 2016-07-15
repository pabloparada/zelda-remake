using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public enum TransitionType
    {
        MOVE_SCENE_LEFT,
        MOVE_SCENE_DOWN,
        MOVE_SCENE_RIGHT,
        MOVE_SCENE_UP,
        BLINK,
        OPEN_INVENTORY,
        CLOSE_INVENTORY
    }

    public class Portal : Entity
    {
        public bool collideOnHit = true;
        public string targetMap;
        public TransitionType transitionType;
        public Vector2 targetPosition;
        
        public Portal(Vector2 p_position, Vector2 p_size)
        {
            state = State.ACTIVE;
            position = p_position;
            size = p_size;
            hitboxSize = p_size;
            hitboxOffset = Vector2.Zero;
            UpdateAABB();
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            base.DebugDraw(p_spriteBatch);
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(position, size, parentPosition), Color.Blue, 3.0f);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            base.Update(p_delta);
        }
    }
}
