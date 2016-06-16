using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public enum TransitionType
    {
        MOVE_SCENE_LEFT,
        MOVE_SCENE_DOWN,
        MOVE_SCENE_RIGHT,
        MOVE_SCENE_UP,
        BLINK
    }
    public class Portal : Entity
    {
        private Vector2 _position;
        private Vector2 _size;
        public AABB aabb;
        private Rectangle _hitbox;

        public bool collideOnHit = true;
        public string targetMap;
        public TransitionType transitionType;
        public Vector2 targetPosition;
        
        public Portal(Vector2 p_position, Vector2 p_size)
        {
            state = State.ACTIVE;
            _position = p_position;
            _size = p_size;
            aabb = new AABB(_position, _position + _size);
            _hitbox = aabb.ToRectangle((int)_size.X, (int)_size.Y);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            Rectangle debugHitbox = new Rectangle((int)(parentScenePosition.X + _hitbox.X) * Main.s_scale,
                   (int)(parentScenePosition.Y + _hitbox.Y) * Main.s_scale + 48 * Main.s_scale,
                    _hitbox.Width * Main.s_scale, _hitbox.Height * Main.s_scale);
            p_spriteBatch.DrawRectangle(debugHitbox, Color.Blue, 3.0f);
            base.DebugDraw(p_spriteBatch);
        }
        public override void Update(float p_delta, Collider p_collider)
        {
            base.Update(p_delta);
        }
    }
}
