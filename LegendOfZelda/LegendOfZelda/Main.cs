using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Main : Game
    {
        public static int s_scale = 4;
        public static Game s_game;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private World world;

        public Main()
        {
            s_game = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 224 * s_scale;
            graphics.PreferredBackBufferWidth = 256 * s_scale;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            world = new World(spriteBatch, graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            world.Update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone, null, null);

            world.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
