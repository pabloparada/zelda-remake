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
        private Collider _collider;

        private SpriteFont _font;

        public Scene(RootObject p_rootObject, Player p_player,params Entity[] p_entities)
        {
            _player = p_player;
            _rootObject = p_rootObject;
            _collider = new Collider(p_rootObject);

            Entities = new List<Entity>(p_entities);
            Entities.Add(_player);
            
            _worldTileSet = Main.s_game.Content.Load<Texture2D>("zelda-tileset");
            _collisionMask = Main.s_game.Content.Load<Texture2D>("CollisionMaskTileSet");

            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayer(_rootObject, "TileMap"), p_spriteBatch, _worldTileSet, 1f);
            Entities.ForEach(e => e.Draw(p_spriteBatch));
            DrawTileMap(RootObjectUtil.GetLayer(_rootObject, "TileMapForeground"), p_spriteBatch, _worldTileSet, 1f);
          
            base.Draw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayer(_rootObject, "CollisionMask"), p_spriteBatch, _collisionMask, 0.35f);
            DrawCollisionMap(p_spriteBatch);
            Entities.ForEach(e => e.DebugDraw(p_spriteBatch));
            
            base.DebugDraw(p_spriteBatch);
        }
        public override void Update(float delta)
        {
            Entities.ForEach(e => e.Update(delta, _collider));

            base.Update(delta);
        }

        private void DrawCollisionMap(SpriteBatch p_spriteBatch)
        {
            foreach (var c in _collider.Collisions)
            {
                var oldRect = c.ToRectangle();
                var rect = c.ToRectangle();

                rect.X *= Main.s_scale;
                rect.Y = rect.Y * Main.s_scale + Main.s_scale * 48;
                rect.Width *= Main.s_scale;
                rect.Height *= Main.s_scale;

                p_spriteBatch.DrawRectangle(rect, Color.DarkRed, 3.0f);
                p_spriteBatch.DrawString(_font, "X:" + oldRect.X + " Y:" + oldRect.Y, new Vector2(rect.X, rect.Y), Color.Black);
            }
        }

        private void DrawTileMap(Layer p_layer, SpriteBatch p_spriteBatch, Texture2D p_tileSet, float p_alpha)
        {
            Rectangle __destinationRect = new Rectangle(0, Main.s_scale * 48, Main.s_scale * 16, Main.s_scale * 16);
            Rectangle __sourceRect = new Rectangle(0, 0, 16, 16);
            for (int i = 0; i < p_layer.data.Count; i++)
            {
                if (p_layer.data[i] >= 0)
                {
                    __sourceRect.X = (p_layer.data[i] % (p_tileSet.Width / 16)) * 16;
                    __sourceRect.Y = (p_layer.data[i] / (p_tileSet.Height / 16)) * 16;
                    p_spriteBatch.Draw(p_tileSet, __destinationRect, __sourceRect, new Color(1f, 1f, 1f, p_alpha));
                }
                __destinationRect.X += Main.s_scale * 16;
                if (__destinationRect.X == Main.s_scale * 256)
                {
                    __destinationRect.X = 0;
                    __destinationRect.Y += Main.s_scale * 16;
                }
            }
        }
    }
}
