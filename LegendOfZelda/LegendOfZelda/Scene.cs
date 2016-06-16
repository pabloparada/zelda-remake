using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Scene : Entity
    {
        public event Action<TransitionType, string> OnPortalEnter;
        private List<Entity> Entities { get; }

        public Player Player { get; set; }
        private List<Portal> _portals;

        private RootObject _rootObject;
        private Texture2D _worldTileSet;
        private Texture2D _collisionMask;
        public Vector2 scenePosition = new Vector2(0f, 0f);
        private Collider _collider;

        private SpriteFont _font;

        public Scene(RootObject p_rootObject, Player p_player,params Entity[] p_entities)
        {
            Player = p_player;
            _rootObject = p_rootObject;
            _collider = new Collider(p_rootObject);

            SetPortals(RootObjectUtil.GetLayerByName(p_rootObject, "Portals"));

            Entities = new List<Entity>(p_entities);
            Entities.Add(Player);
            _portals.ForEach(__portal => Entities.Add(__portal));

            _worldTileSet = Main.s_game.Content.Load<Texture2D>("zelda-tileset");
            _collisionMask = Main.s_game.Content.Load<Texture2D>("CollisionMaskTileSet");

            _font = Main.s_game.Content.Load<SpriteFont>("DebugFontFace");
        }
        private void SetPortals(Layer p_layer)
        {
            _portals = new List<Portal>();
            if (p_layer == null)
            {
                Console.WriteLine("PORTALS LAYER NOT FOUND");
                return;
            }
            Portal __tempPortal;
            foreach (Object p_obj in p_layer.objects)
            {
                __tempPortal = new Portal(new Vector2(p_obj.x,p_obj.y), new Vector2(p_obj.width, p_obj.height));
                __tempPortal.transitionType = (TransitionType)p_obj.properties.TransitionType;
                if (p_obj.properties.TransitionType == (int)TransitionType.BLINK)
                    __tempPortal.collideOnHit = false;
                __tempPortal.targetMap = p_obj.properties.TargetMap;
                _portals.Add(__tempPortal);
            }
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayerByName(_rootObject, "TileMap"), p_spriteBatch, _worldTileSet, 1f);
            foreach(Entity __e in Entities)
                if (__e.state != State.DISABLED)
                    __e.Draw(p_spriteBatch);
            DrawTileMap(RootObjectUtil.GetLayerByName(_rootObject, "TileMapForeground"), p_spriteBatch, _worldTileSet, 1f);
          
            base.Draw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayerByName(_rootObject, "CollisionMask"), p_spriteBatch, _collisionMask, 0.35f);
            DrawCollisionMap(p_spriteBatch);

            foreach (Entity __e in Entities)
                if (__e.state != State.DISABLED)
                    __e.DebugDraw(p_spriteBatch);

            base.DebugDraw(p_spriteBatch);
        }
        public void RemoveEntity(Entity p_ent)
        {
            Entities.Remove(p_ent);
        }
        public override void Update(float delta)
        {
            if (state == State.DRAW_ONLY)
                foreach (Entity __e in Entities)
                {
                    __e.parentScenePosition = scenePosition;
                }
            else
            {
                foreach (Entity __e in Entities)
                {
                    if (__e.state != State.ACTIVE)
                        continue;

                    __e.parentScenePosition = scenePosition;
                    __e.Update(delta, _collider);
                }
                CheckPortalCollision();
            }
            base.Update(delta);
        }
        private void CheckPortalCollision()
        {
            foreach (Portal __portal in _portals)
            {
                if (__portal.state != State.ACTIVE)
                    continue;
                if (__portal.collideOnHit)
                {
                    if (_collider.PointInsideRectangle(Player.aabb.Min, __portal.aabb.Min, __portal.aabb.Max)
                        || _collider.PointInsideRectangle(Player.aabb.Max, __portal.aabb.Min, __portal.aabb.Max)
                        || _collider.PointInsideRectangle(Player.aabb.TopRight, __portal.aabb.Min, __portal.aabb.Max)
                        || _collider.PointInsideRectangle(Player.aabb.BottomLeft, __portal.aabb.Min, __portal.aabb.Max))
                    {
                        Console.WriteLine(_portals.IndexOf(__portal) + "Collide");
                        OnPortalEnter?.Invoke(__portal.transitionType, __portal.targetMap);
                        return;
                    }
                }
                else if (_collider.PointInsideRectangle(Player.aabb.Min, __portal.aabb.Min, __portal.aabb.Max)
                        && _collider.PointInsideRectangle(Player.aabb.Max, __portal.aabb.Min, __portal.aabb.Max))
                {
                    OnPortalEnter?.Invoke(__portal.transitionType, __portal.targetMap);
                    Console.WriteLine(_portals.IndexOf(__portal) + "Collide");
                    return;
                }
            }
        }
        private void DrawCollisionMap(SpriteBatch p_spriteBatch)
        {
            foreach (var __collider in _collider.Collisions)
            {
                Rectangle oldRect = __collider.ToRectangle();
                Rectangle rect = __collider.ToRectangle();

                rect.X = (int)(scenePosition.X + rect.X) * Main.s_scale;
                rect.Y = (int)(scenePosition.Y + rect.Y) * Main.s_scale + Main.s_scale * 48;
                rect.Width *= Main.s_scale;
                rect.Height *= Main.s_scale;
                if (__collider.Mask != CollisionMask.NONE)
                {
                    p_spriteBatch.DrawRectangle(rect, Color.DarkRed, 3.0f);
                    p_spriteBatch.DrawString(_font, "X:" + oldRect.X + " Y:" + oldRect.Y, new Vector2(rect.X, rect.Y), Color.White);
                }
            }
        }

        private void DrawTileMap(Layer p_layer, SpriteBatch p_spriteBatch, Texture2D p_tileSet, float p_alpha)
        {
            if (p_layer == null)
                return;
            Rectangle __destinationRect = new Rectangle((int)(Main.s_scale * scenePosition.X),
                (int)(Main.s_scale * scenePosition.Y) + Main.s_scale * 48,Main.s_scale * 16, Main.s_scale * 16);
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
                if (__destinationRect.X == Main.s_scale * 256 + (int)(Main.s_scale * scenePosition.X))
                {
                    __destinationRect.X = (int)(Main.s_scale * scenePosition.X);
                    __destinationRect.Y += Main.s_scale * 16;
                }
            }
        }
    }
}
