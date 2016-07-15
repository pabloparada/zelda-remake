using System;
using System.Collections.Generic;
using LegendOfZelda.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Items;
using LegendOfZelda.Util;
using LegendOfZelda.Weapons;

namespace LegendOfZelda
{
    public class Scene : Entity
    {
        public event Action<Portal> OnPortalEnter;

        private List<Entity> entities { get; }
        public Player Player { get; set; }

        private List<Portal> _portals;
        private List<Enemy> _enemies;

        private readonly RootObject _rootObject;
        private readonly Texture2D _worldTileSet;
        private readonly Texture2D _collisionMask;
        private readonly Collider _collider;
        private readonly PlayerWeaponManager _weaponManager;

        public Vector2 scenePosition = new Vector2(0f, 48f);

        public Scene(RootObject p_rootObject, Player p_player, params Entity[] p_entities)
        {
            tag = "Scene";
            Player = p_player;
            _rootObject = p_rootObject;
            _collider = new Collider(p_rootObject);

            _weaponManager = new PlayerWeaponManager(p_player);

            entities = new List<Entity>(p_entities);

            SetPortals(RootObjectUtil.GetLayerByName(p_rootObject, "Portals"));
            SetItems(RootObjectUtil.GetLayerByName(p_rootObject, "Items"));
            SetEnemies(RootObjectUtil.GetLayerByName(p_rootObject, "Enemies"));

            entities.Add(Player);
            _portals.ForEach(p_portal => entities.Add(p_portal));
            _enemies.ForEach(p_enemy => entities.Add(p_enemy));

            _worldTileSet = Main.s_game.Content.Load<Texture2D>("TileSet_World");
            _collisionMask = Main.s_game.Content.Load<Texture2D>("TileSet_CollisionMask");
        }

        private void SetPortals(Layer p_layer)
        {
            _portals = new List<Portal>();

            if (p_layer == null)
            {
                Console.WriteLine("PORTALS LAYER NOT FOUND");
                return;
            }

            foreach (var __obj in p_layer.objects)
            {
                var __tempPortal = new Portal(new Vector2(__obj.x, __obj.y), new Vector2(__obj.width, __obj.height))
                {
                    transitionType = (TransitionType) __obj.properties.TransitionType
                };

                if (__obj.properties.TransitionType == (int)TransitionType.BLINK)
                {
                    __tempPortal.collideOnHit = false;
                    __tempPortal.targetPosition = new Vector2(__obj.properties.TargetPositionX, __obj.properties.TargetPositionY);
                }

                __tempPortal.targetMap = __obj.properties.TargetMap;
                _portals.Add(__tempPortal);
            }
        }

        private void SetEnemies(Layer p_layer)
        {
            _enemies = new List<Enemy>();

            if (p_layer == null) return;

            foreach (Object __obj in p_layer.objects)
                _enemies.Add(EnemyFactory.CreateEnemyByObject(__obj));
        }

        private void SetItems(Layer p_layer)
        {
            if (p_layer == null)
            {
                Console.WriteLine("ITEMS LAYER NOT FOUND");

                return;
            }
            foreach (var __obj in p_layer.objects)
            {
                var __tempItem = Item.SpawnItem(__obj);

                __tempItem.name = __obj.name;
                __tempItem.type = EntityType.ITEM;
                __tempItem.OnDestroyEntity += RemoveEntity;

                entities.Add(__tempItem);
            }
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayerByName(_rootObject, "TileMap"), p_spriteBatch, _worldTileSet, 1f);

            foreach (var __e in entities)
            {
                if (__e.state != State.DISABLED)
                {
                    __e.Draw(p_spriteBatch);
                }
            }

            _weaponManager.Draw(p_spriteBatch);

            base.Draw(p_spriteBatch);
        }

        public void DrawForeground(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayerByName(_rootObject, "TileMapForeground"), p_spriteBatch, _worldTileSet, 1f);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            DrawTileMap(RootObjectUtil.GetLayerByName(_rootObject, "CollisionMask"), p_spriteBatch, _collisionMask, 0.35f);
            DrawCollisionMap(p_spriteBatch);

            foreach (Entity __e in entities)
            {
                if (__e.state != State.DISABLED)
                {
                    __e.DebugDraw(p_spriteBatch);
                }
            }

            _weaponManager.DebugDraw(p_spriteBatch);

            base.DebugDraw(p_spriteBatch);
        }

        public void RemoveEntity(Entity p_ent)
        {
            entities.Remove(p_ent);
        }

        public override void Update(float p_delta)
        {
            if (state == State.DRAW_ONLY)
            {
                foreach (var __e in entities)
                {
                    __e.parentPosition = scenePosition;
                }
            }
            else
            {
                foreach (var __e in entities)
                {
                    if (__e.state != State.ACTIVE) continue;

                    __e.parentPosition = scenePosition;
                    __e.Update(p_delta, _collider);
                }

                _weaponManager.Update(p_delta, _collider);

                CheckCollisions();
                CheckPortalCollision();
            }

            base.Update(p_delta);
        }

        private void CheckCollisions()
        {
            for (var __i = 0; __i < entities.Count - 2; __i++)
                for (var __j = __i + 1; __j < entities.Count - 1; __j++)
                {
                    if (_collider.IsIntersectingRectangle(entities[__i].aabb, entities[__j].aabb))
                    {
                        entities[__i].OnCollide(entities[__j]);
                        entities[__j].OnCollide(entities[__i]);
                    }
                }
        }

        private void CheckPortalCollision()
        {
            foreach (var __portal in _portals)
            {
                if (__portal.state != State.ACTIVE) continue;

                if (__portal.collideOnHit)
                {
                    if (_collider.IsIntersectingRectangle(__portal.aabb, Player.aabb))
                    {
                        OnPortalEnter?.Invoke(__portal);

                        return;
                    }
                }
                else if (_collider.IsPointInsideRectangle(Player.aabb.Min, __portal.aabb.Min, __portal.aabb.Max) &&
                         _collider.IsPointInsideRectangle(Player.aabb.Max, __portal.aabb.Min, __portal.aabb.Max))
                {
                    OnPortalEnter?.Invoke(__portal);

                    return;
                }
            }
        }

        private void DrawCollisionMap(SpriteBatch p_spriteBatch)
        {
            foreach (var __collider in _collider.Collisions)
            {
                var __oldRect = __collider.ToRectangle();
                var __rect = __collider.ToRectangle();

                __rect.X = (int)(scenePosition.X + __rect.X) * Main.s_scale;
                __rect.Y = (int)(scenePosition.Y + __rect.Y) * Main.s_scale;
                __rect.Width *= Main.s_scale;
                __rect.Height *= Main.s_scale;

                if (__collider.Mask != CollisionMask.NONE)
                {
                    p_spriteBatch.DrawRectangle(__rect, Color.DarkRed, 3.0f);
                    p_spriteBatch.DrawString(Main.s_debugFont, "X:" + __oldRect.X + " Y:" + __oldRect.Y, new Vector2(__rect.X, __rect.Y), Color.White);
                }
            }
        }

        private void DrawTileMap(Layer p_layer, SpriteBatch p_spriteBatch, Texture2D p_tileSet, float p_alpha)
        {
            if (p_layer == null) return;

            var __destinationRect = new Rectangle((int)(Main.s_scale * scenePosition.X),
                                                  (int)(Main.s_scale * scenePosition.Y), Main.s_scale * 16, Main.s_scale * 16);

            var __sourceRect = new Rectangle(0, 0, 16, 16);

            foreach (var __t in p_layer.data)
            {
                if (__t >= 0)
                {
                    __sourceRect.X = (__t % (p_tileSet.Width / 16)) * 16;
                    __sourceRect.Y = (__t / (p_tileSet.Height / 16)) * 16;
                    p_spriteBatch.Draw(p_tileSet, __destinationRect, __sourceRect, new Color(1.0f, 1.0f, 1.0f, p_alpha));
                }

                __destinationRect.X += Main.s_scale * 16;

                if (__destinationRect.X == Main.s_scale * 256 + (int) (Main.s_scale * scenePosition.X))
                {
                    __destinationRect.X = (int)(Main.s_scale * scenePosition.X);
                    __destinationRect.Y += Main.s_scale * 16;
                }
            }
        }
    }
}
