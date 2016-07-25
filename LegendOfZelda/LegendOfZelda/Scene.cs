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
        private List<Entity> entitiesToRemove;
        public Player player { get; set; }

        public List<Door>       doors;
        public List<Portal>     portals;
        private List<Enemy>     _enemies;
        private List<Weapon>    _weapons;

        private readonly RootObject _rootObject;
        private readonly Texture2D _worldTileSet;
        private readonly Texture2D _collisionMask;
        private readonly Collider _collider;
        private readonly PlayerWeaponManager _playerWeaponManager;
        private readonly EnemyWeaponManager _enemyWeaponManager;

        public Vector2 scenePosition = new Vector2(0f, 48f);

        public Scene(RootObject p_rootObject, Player p_player, params Entity[] p_entities)
        {
            tag = "Scene";
            player = p_player;
            _rootObject = p_rootObject;
            _collider = new Collider(p_rootObject);

            entities = new List<Entity>(p_entities);
            entitiesToRemove = new List<Entity>();

            _playerWeaponManager = new PlayerWeaponManager(p_player);
            _playerWeaponManager.OnLinkSwordDie += _playerWeaponManager_OnLinkSwordDie;
            _enemyWeaponManager = new EnemyWeaponManager();

            SetPortals(RootObjectUtil.GetLayerByName(p_rootObject, "Portals"));
            SetItems(RootObjectUtil.GetLayerByName(p_rootObject, "Items"));
            SetEnemies(RootObjectUtil.GetLayerByName(p_rootObject, "Enemies"));
            SetDoors(RootObjectUtil.GetLayerByName(p_rootObject, "Doors"));
            entities.Add(player);

            portals.ForEach(p_portal => entities.Add(p_portal));
            _enemies.ForEach(p_enemy => entities.Add(p_enemy));

            if (_enemies.Count == 0)
            {
                AllEnemiesDead();
            }
                
            _worldTileSet = Main.s_game.Content.Load<Texture2D>("TileSet_World");
            _collisionMask = Main.s_game.Content.Load<Texture2D>("TileSet_CollisionMask");

            SoundManager.instance.Play(World.IsOpenWorld() ? SoundType.OVERWORLD : SoundType.DUNGEON, true);
            SoundManager.instance.StopAndDispose(World.IsOpenWorld() ? SoundType.DUNGEON : SoundType.OVERWORLD);
        }

        private void _playerWeaponManager_OnLinkSwordDie(Vector2 obj)
        {
            CreateExplosion(obj, 2);
        }
        private void SetDoors(Layer p_layer)
        {
            doors = new List<Door>();
            if (p_layer == null)
            {
                Console.WriteLine("DOORS LAYER NOT FOUND");
                return;
            }
            foreach (var __obj in p_layer.objects)
            {
                var __tempDoor = new Door(__obj);
                __tempDoor.OnDoorOpen += OnDoorOpen;
                if (World.s_saveState.HasDoor(__obj.name, World.mapName))
                    OnDoorOpen(__tempDoor);
                doors.Add(__tempDoor);
            }
        }

        private void OnDoorOpen(Door p_door)
        {
            World.s_saveState.AddDoorToRoom(p_door.name, World.mapName);
            if (p_door.doorSide == 0)
            {
                Layer __layer = RootObjectUtil.GetLayerByName(_rootObject, "TileMap");
                __layer.data[p_door.tileToOpen] = 417;
                __layer = RootObjectUtil.GetLayerByName(_rootObject, "CollisionMask");
                __layer.data[p_door.tileToOpen] = 0;
                _collider.ChangeAABBMaskType(p_door.tileToOpen, CollisionMask.NONE);
            }
            else if (p_door.doorSide == 1)
            {
                Layer __layer = RootObjectUtil.GetLayerByName(_rootObject, "TileMap");
                __layer.data[p_door.tileToOpen] = 397;
                __layer.data[p_door.tileToOpen + 1] = 398;
                __layer = RootObjectUtil.GetLayerByName(_rootObject, "CollisionMask");
                __layer.data[p_door.tileToOpen] = 8;
                __layer.data[p_door.tileToOpen + 1] = 9;
                _collider.ChangeAABBMaskType(p_door.tileToOpen, CollisionMask.HALF_LEFT);
                _collider.ChangeAABBMaskType(p_door.tileToOpen + 1, CollisionMask.HALF_RIGHT);
            }
            else if  (p_door.doorSide == 2)
            {
                Layer __layer = RootObjectUtil.GetLayerByName(_rootObject, "TileMap");
                __layer.data[p_door.tileToOpen] = 387;
                __layer = RootObjectUtil.GetLayerByName(_rootObject, "CollisionMask");
                __layer.data[p_door.tileToOpen] = 0;
                _collider.ChangeAABBMaskType(p_door.tileToOpen, CollisionMask.NONE);
            }

            SoundManager.instance.Play(SoundType.PLAYER_HITTED);
        }

        private void SetPortals(Layer p_layer)
        {
            portals = new List<Portal>();

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
                portals.Add(__tempPortal);
            }
        }

        private void SetEnemies(Layer p_layer)
        {
            _enemies = new List<Enemy>();

            if (p_layer == null) return;

            foreach (Object __obj in p_layer.objects)
            {
                if (World.s_saveState.HasEnemy(__obj.name, World.mapName))
                    continue;
                var __enemy = EnemyFactory.CreateEnemyByObject(__obj, player, _collider);

                __enemy.OnDestroyEntity += RemoveEntity;
                __enemy.name = __obj.name;
                if (__enemy is Aquamentus)
                {
                    var __aqua = (Aquamentus) __enemy;

                    __aqua.AddWeaponsToManager += _enemyWeaponManager.AddWeapons;
                    __aqua.RemoveWeaponFromManagerByWeaponId += _enemyWeaponManager.RemoveWeaponFromManagerByWeaponId;
                }
                else
                {
                    __enemy.AddWeaponToManager += _enemyWeaponManager.AddWeapon;
                    __enemy.RemoveWeaponFromManager += _enemyWeaponManager.RemoveWeapon;
                }

                _enemies.Add(__enemy);

                CreateExplosion(new Vector2(__obj.x, __obj.y), 0);
            }
        }
        private void CreateExplosion(Vector2 p_pos, int p_explosionType)
        {
            if (p_explosionType == 0)
            {
                entities.Add(new SpawnExplosion(p_pos));
                entities[entities.Count - 1].OnDestroyEntity += RemoveEntity;
            }
            else if (p_explosionType == 1)
            {
                entities.Add(new DeathExplosion(p_pos));
                entities[entities.Count - 1].OnDestroyEntity += RemoveEntity;
            }
            else if (p_explosionType == 2)
            {
                entities.Add(new SwordExplosion(p_pos, new Vector2(-1f, 1f)));
                entities[entities.Count - 1].OnDestroyEntity += RemoveEntity;
                entities.Add(new SwordExplosion(p_pos, new Vector2(1f, 1f)));
                entities[entities.Count - 1].OnDestroyEntity += RemoveEntity;
                entities.Add(new SwordExplosion(p_pos, new Vector2(-1f, -1f)));
                entities[entities.Count - 1].OnDestroyEntity += RemoveEntity;
                entities.Add(new SwordExplosion(p_pos, new Vector2(1f, -1f)));
                entities[entities.Count - 1].OnDestroyEntity += RemoveEntity;
            }

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
                if (World.s_saveState.HasItem(__obj.name, World.mapName))
                    continue;
                var __tempItem = Item.SpawnItem(__obj);

                if (__tempItem == null)
                    continue;

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

            _playerWeaponManager.Draw(p_spriteBatch);
            _enemyWeaponManager.Draw(p_spriteBatch);
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

            _playerWeaponManager.DebugDraw(p_spriteBatch);
            _enemyWeaponManager.DebugDraw(p_spriteBatch);

            base.DebugDraw(p_spriteBatch);
        }

        public void RemoveEntity(Entity p_ent)
        {
            entitiesToRemove.Add(p_ent);
            if (p_ent.type == EntityType.ENEMY)
            {
                CreateExplosion(p_ent.position, 1);
                World.s_saveState.AddEnemyToRoom(p_ent.name, World.mapName);
            }
            else if (p_ent.type == EntityType.ITEM)
                World.s_saveState.AddItemToRoom(p_ent.name, World.mapName);
        }
        private void RemoveEntities()
        {
            foreach (Entity __en in entitiesToRemove)
            {
                entities.Remove(__en);
                if (__en.type == EntityType.ENEMY)
                {
                    _enemies.Remove((Enemy)__en);
                    if (_enemies.Count == 0)
                        AllEnemiesDead();
                }
            }
            entitiesToRemove.Clear();
        }
        private void AllEnemiesDead()
        {
            Console.WriteLine("AllDead");
            foreach (Door __door in doors)
                __door.AllDead();
            foreach (Entity __en in entities)
                if (__en.type == EntityType.ITEM)
                    ((Item)__en).AllDead();
        }
        public override void Update(float p_delta)
        {
            RemoveEntities();
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

                _playerWeaponManager.Update(p_delta, _collider);
                _enemyWeaponManager.Update(p_delta, _collider);

                var __tmpEntities = new List<Entity>(entities);

                __tmpEntities.AddRange(_playerWeaponManager.weapons);
                __tmpEntities.AddRange(_enemyWeaponManager.weapons);
                __tmpEntities.AddRange(doors);

                CheckCollisions(__tmpEntities);
                CheckPortalCollision();
            }

            base.Update(p_delta);
        }

        private void CheckCollisions(List<Entity> p_entities)
        {
            for (var __i = 0; __i < p_entities.Count; __i++)
                for (var __j = __i + 1; __j < p_entities.Count; __j++)
                {
                    var __entity1 = p_entities[__i];
                    var __entity2 = p_entities[__j];

                    if (ShouldSkipCollisionCheck(__entity1, __entity2))
                    {
                        continue;
                    }
                    if (__entity1.state != State.ACTIVE || __entity2.state != State.ACTIVE)
                        continue;

                    if (_collider.IsIntersectingRectangle(__entity1.aabb, __entity2.aabb))
                    {
                        __entity1.OnCollide(__entity2);
                        __entity2.OnCollide(__entity1);
                    }
                }
        }

        // skip weapon vs item | player vs weapon | weapon vs weapon
        private bool ShouldSkipCollisionCheck(Entity p_entity1, Entity p_entity2)
        {
            return p_entity1.type == EntityType.WEAPON && p_entity2.type == EntityType.WEAPON ||
                   p_entity1.type == EntityType.WEAPON && p_entity2.type == EntityType.ITEM ||
                   p_entity2.type == EntityType.WEAPON && p_entity1.type == EntityType.ITEM;
        }

        private void CheckPortalCollision()
        {
            foreach (var __portal in portals)
            {
                if (__portal.state != State.ACTIVE) continue;

                if (player.immunityTimeAferHit >= 0.0f) return;

                if (__portal.collideOnHit)
                {
                    if (_collider.IsIntersectingRectangle(__portal.aabb, player.aabb))
                    {
                        DisableAllButPlayer();
                        OnPortalEnter?.Invoke(__portal);

                        return;
                    }
                }
                else if (_collider.IsPointInsideRectangle(player.movementAABB.Min, __portal.aabb.Min, __portal.aabb.Max) &&
                         _collider.IsPointInsideRectangle(player.movementAABB.Max, __portal.aabb.Min, __portal.aabb.Max))
                {
                    DisableAllButPlayer();
                    OnPortalEnter?.Invoke(__portal);
                    
                    return;
                }
            }
        }
        private void DisableAllButPlayer()
        {
            foreach (Entity __en in entities)
                if (__en.type != EntityType.PLAYER)
                    __en.state = State.DISABLED;
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
