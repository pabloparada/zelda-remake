using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Scene : Entity
    {
        private List<Entity> Entities { get; }
        private Player _player;

        private RootObject _rootObject;
        private Texture2D _worldTileSet;
        private Texture2D _collisionMask;
        public Vector2 scenePosition = new Vector2(0f, 0f);
        public Scene(RootObject p_rootObject, Player p_player,params Entity[] p_entities)
        {
            _player = p_player;
            Entities = new List<Entity>(p_entities);
            Entities.Add(_player);

            foreach (Entity __entity in Entities)
                __entity.scene = this;
            

            _rootObject = p_rootObject;
            _worldTileSet = Main.s_game.Content.Load<Texture2D>("zelda-tileset");
            _collisionMask = Main.s_game.Content.Load<Texture2D>("CollisionMaskTileSet");
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            
            foreach (Layer __layer in _rootObject.layers)
            {
                if (__layer.name == "TileMap")
                {
                    Rectangle __destinationRect = new Rectangle(0, Main.s_scale * 48, Main.s_scale * 16, Main.s_scale * 16);
                    Rectangle __sourceRect = new Rectangle(0, 0, 16, 16);
                    for (int i = 0; i < __layer.data.Count; i ++)
                    {
                        __sourceRect.X = (__layer.data[i] % 32)* 16;
                        __sourceRect.Y = (__layer.data[i]/32) * 16;
                        p_spriteBatch.Draw(_worldTileSet,__destinationRect, __sourceRect, Color.White);

                        __destinationRect.X += Main.s_scale * 16;
                        if (__destinationRect.X == Main.s_scale * 256)
                        {
                            __destinationRect.X = 0;
                            __destinationRect.Y += Main.s_scale * 16;
                        }
                    }
                }
            }
            Entities.ForEach(e => e.Draw(p_spriteBatch));
            base.Draw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {

            foreach (Layer __layer in _rootObject.layers)
            {
                if (__layer.name == "CollisionMask")
                {
                    Rectangle __destinationRect = new Rectangle(0, Main.s_scale * 48, Main.s_scale * 16, Main.s_scale * 16);
                    Rectangle __sourceRect = new Rectangle(0, 0, 16, 16);
                    for (int i = 0; i < __layer.data.Count; i++)
                    {
                        __sourceRect.X = (__layer.data[i] % 4) * 16;
                        __sourceRect.Y = (__layer.data[i] / 4) * 16;
                        p_spriteBatch.Draw(_collisionMask, __destinationRect, __sourceRect, new Color(1f, 1f, 1f, 0.35f));

                        __destinationRect.X += Main.s_scale * 16;
                        if (__destinationRect.X == Main.s_scale * 256)
                        {
                            __destinationRect.X = 0;
                            __destinationRect.Y += Main.s_scale * 16;
                        }
                    }
                }
            }
            Entities.ForEach(e => e.DebugDraw(p_spriteBatch));
            base.DebugDraw(p_spriteBatch);
        }
        public override void Update(float delta)
        {
            Entities.ForEach(e => e.Update(delta));

            base.Update(delta);
        }
    }
}
