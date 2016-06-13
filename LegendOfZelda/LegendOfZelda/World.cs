using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class World : Entity
    {
        public Scene CurrentScene { get; private set; }
        private Scene previousScene;

        public HUD hud;

        private TiledReader _tileReader;
        
        public World(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager)
        {
            _tileReader = new TiledReader();
            hud = new HUD();
            //CurrentScene = new Scene(_tileReader.LoadTiledJson("Dungeon_1-0"),new Player(graphicsDeviceManager));
            CurrentScene = new Scene(_tileReader.LoadTiledJson("Room_7-7"), new Player(graphicsDeviceManager));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(spriteBatch);
            hud.DrawHUD(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            CurrentScene.DebugDraw(p_spriteBatch);

            base.DebugDraw(p_spriteBatch);
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
