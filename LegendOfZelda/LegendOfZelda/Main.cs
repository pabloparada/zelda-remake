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
        public static GameState s_gameState = GameState.PLAYING;
        public static SpriteFont s_debugFont;

        public enum GameState
        {
            PLAYING,
            INVENTORY,
            PAUSE
        }

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
            GraphicAssets.LoadContent();
            world = new World();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            s_debugFont = Content.Load<SpriteFont>("DebugFontFace");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            inputManager.UpdateState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (InputManager.GetKeyChange(Keys.F1)) s_isOnDebugMode = !s_isOnDebugMode;

            if (InputManager.GetKeyChange(Keys.F2)) ChangeScale();

            if (InputManager.GetKeyChange(Keys.F3)) world.OpenCloseInventory();

            var __delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            world.Update(__delta);
            inputManager.UpdateOldState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime p_delta)
        {
            if (!IsActive) return;

            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);

            world.Draw(spriteBatch);

            if (s_isOnDebugMode) world.DebugDraw(spriteBatch);

            spriteBatch.End();

            base.Draw(p_delta);
        }

        private void ChangeScale()
        {
            s_scale += 1;

            if (s_scale == 5) s_scale = 1;
           
            graphics.PreferredBackBufferHeight = 224 * s_scale;
            graphics.PreferredBackBufferWidth = 256 * s_scale;
            graphics.ApplyChanges();
        }
    }
}
