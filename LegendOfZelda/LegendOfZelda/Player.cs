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
            position = new Vector2(128, 48);
            velocity = new Vector2(80.0f, 80.0f);
            direction = new Vector2(0, 0);
            linkSpriteSize = new Vector2(12, 12);
            hitbox = new Rectangle(position.ToPoint(), linkSpriteSize.ToPoint());
            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                direction.X = 0;
                direction.Y = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction.X = -1;
                direction.Y = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction.X = 1;
                direction.Y = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                direction.X = 0;
                direction.Y = 1;
            }
            else
            {
                direction.X = 0;
                direction.Y = 0;
            }

            Vector2 __tempPos = position + direction * velocity * p_delta;

            var aabb = new Collider.AABB(__tempPos, __tempPos + linkSpriteSize);

            if (p_collider.IsColliding(aabb))
            {
                _isColliding = true;
            }
            else
            {
                _isColliding = false;
            }

            position += direction  * velocity * p_delta;

            hitbox.X = (int) position.X;
            hitbox.Y = (int) position.Y;

            base.Update(p_delta);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            var rect = new Rectangle((int) (position.X * Main.s_scale), (int) (position.Y * Main.s_scale + 48 * Main.s_scale), hitbox.Width * Main.s_scale, hitbox.Height * Main.s_scale);
            p_spriteBatch.FillRectangle(rect, Color.Green);
            base.DebugDraw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            var debugHitbox = new Rectangle(hitbox.X * Main.s_scale, hitbox.Y * Main.s_scale + 48 * Main.s_scale, hitbox.Width * Main.s_scale, hitbox.Height * Main.s_scale);
            p_spriteBatch.DrawRectangle(debugHitbox, Color.ForestGreen, 1.0f);

            if (_isColliding)
                p_spriteBatch.DrawRectangle(debugHitbox, Color.Red, 1.0f);

            var __msgPos = new Vector2(position.X * Main.s_scale, position.Y * Main.s_scale + 48 * Main.s_scale);
            p_spriteBatch.DrawString(_font, "X:" + (int) position.X + " Y:" + (int) position.Y, __msgPos, Color.Black);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
