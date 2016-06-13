using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Player : Entity
    {
        
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 direction;

        private Rectangle hitbox;

        private Vector2 linkSpriteSize;

        private SpriteFont _font;

        private bool _isColliding;

        public Player(GraphicsDeviceManager p_graphicsDeviceManager)
        {
            position = new Vector2(148, 100);
            velocity = new Vector2(80.0f, 80.0f);
            direction = new Vector2(0, 0);
            linkSpriteSize = new Vector2(12, 12);
            hitbox = new Rectangle(position.ToPoint(), linkSpriteSize.ToPoint());
            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            Direction __dir = Direction.UP;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                direction.X = 0;
                direction.Y = -1;
                __dir = Direction.UP;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction.X = -1;
                direction.Y = 0;
                __dir = Direction.LEFT;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction.X = 1;
                direction.Y = 0;
                __dir = Direction.RIGHT;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                direction.X = 0;
                direction.Y = 1;
                __dir = Direction.DOWN;
            }
            else
            {
                direction.X = 0;
                direction.Y = 0;
            }

            Vector2 __tempPos = position + direction * velocity * p_delta;
            AABB __tempAabb = new AABB(__tempPos, __tempPos + linkSpriteSize);
            if (direction.X != 0 || direction.Y !=0)
                _isColliding = p_collider.IsColliding(__tempAabb, __dir);

            float __reachFraction = 1.0f;
            float __maxReach = 0f;
            if (_isColliding)
            {
                __reachFraction = p_delta * 0.5f;
                for (var i = 0; i < 4; i++)
                {
                    __tempPos = position + (direction * velocity * p_delta * __reachFraction);
                    _isColliding = p_collider.IsColliding(new AABB(__tempPos, __tempPos + linkSpriteSize), __dir);
                    if (_isColliding)
                        __reachFraction -= 1f / (float)Math.Pow(2, i + 2);
                    else
                    {
                        __reachFraction += 1f / (float)Math.Pow(2, i + 2);
                        __maxReach = __reachFraction;
                    }
                }
            }
            else
                __maxReach = 1f;

            if (__maxReach > 0f)
            {
                position += direction * velocity * p_delta * __maxReach;
                position = new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y));
                hitbox.X =  (int)position.X;
                hitbox.Y = (int)position.Y;
            }

            base.Update(p_delta);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            Rectangle rect = new Rectangle((int)((parentScenePosition.X+position.X) * Main.s_scale),
                (int) ((parentScenePosition.Y + position.Y) * Main.s_scale + 48 * Main.s_scale), 
                hitbox.Width * Main.s_scale, hitbox.Height * Main.s_scale);
            p_spriteBatch.FillRectangle(rect, Color.Green);
            base.DebugDraw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            Rectangle debugHitbox = new Rectangle((int)(parentScenePosition.X + hitbox.X) * Main.s_scale,
                (int)(parentScenePosition.Y + hitbox.Y) * Main.s_scale + 48 * Main.s_scale, 
                hitbox.Width * Main.s_scale, hitbox.Height * Main.s_scale);
            p_spriteBatch.DrawRectangle(debugHitbox, Color.ForestGreen, 1.0f);

            if (_isColliding)
                p_spriteBatch.DrawRectangle(debugHitbox, Color.Red, 1.0f);

            var __msgPos = new Vector2((int)(parentScenePosition.X + position.X) * Main.s_scale,
                (int)(parentScenePosition.Y + position.Y) * Main.s_scale + 48 * Main.s_scale);
            p_spriteBatch.DrawString(_font, "X:" + (int) position.X + " Y:" + (int) position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
