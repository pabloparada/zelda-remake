using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Link : Entity
    {
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 direction;

        private Rectangle hitbox;

        private Vector2 linkSpriteSize;

        public Link(GraphicsDeviceManager p_graphicsDeviceManager)
        {
            position = new Vector2(10.0f, 10.0f);
            velocity = new Vector2(150.0f, 150.0f);
            direction = new Vector2(0, 0);
            linkSpriteSize = new Vector2(16, 16);
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
