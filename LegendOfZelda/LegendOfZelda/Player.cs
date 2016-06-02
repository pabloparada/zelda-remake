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

       
        public Player(GraphicsDeviceManager p_graphicsDeviceManager)
        {
            position = new Vector2(220.0f, 50.0f) * Main.s_scale;
            velocity = new Vector2(80.0f, 80.0f) * Main.s_scale;
            direction = new Vector2(0, 0);
            linkSpriteSize = new Vector2(12, 12) * Main.s_scale;
        }

        public override void Update(float p_delta)
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
            Vector2 __tempPos = position;
            if (__tempPos.X <= 0 || __tempPos.X + linkSpriteSize.X >= 256 * Main.s_scale)
            {
                System.Console.WriteLine(__tempPos.ToString());
                base.Update(p_delta);
                return;
            }
            position += direction  * velocity * p_delta;

            hitbox = new Rectangle(position.ToPoint(), linkSpriteSize.ToPoint());

            base.Update(p_delta);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(position, linkSpriteSize, Color.Green);
            p_spriteBatch.DrawRectangle(hitbox, Color.Red, 1.0f);

            base.Draw(p_spriteBatch);
        }
    }
}
