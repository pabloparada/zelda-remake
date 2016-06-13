using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class Main : Game
    {
        public static int s_scale = 4;
        public static Game s_game;
        public static bool s_isOnDebugMode = false;

        public InputManager inputManager;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private World world;

        public Main()
        {
            s_game = this;
            inputManager = new InputManager();
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
            if (!IsActive) return;

            inputManager.UpdateState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (InputManager.GetKeyChange(Keys.F1)) s_isOnDebugMode = !s_isOnDebugMode;

            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            world.Update(delta);
            inputManager.UpdateOldState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone, null, null);

            world.Draw(spriteBatch);
            if (s_isOnDebugMode)
                world.DebugDraw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
