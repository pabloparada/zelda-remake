using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class World : Entity
    {
        public Scene CurrentScene { get; private set; }
        private Scene previousScene;

        public World(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager)
        {
            CurrentScene = new Scene(new Link(graphicsDeviceManager));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Update(float delta)
        {
            CurrentScene.Update(delta);

            base.Update(delta);
        }

        public Scene ChangeScene(Scene nextScene)
        {
            previousScene = CurrentScene;
            CurrentScene = nextScene;

            // TODO: translate curr -> next visually

            return previousScene;
        }
    }
}
