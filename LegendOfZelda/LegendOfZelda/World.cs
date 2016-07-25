using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.GUI;

namespace LegendOfZelda
{
    public class World : Entity
    {
        public Scene CurrentScene { get; private set; }
        private Scene previousScene;

        public GUIManager guiManager;

        private TiledReader _tileReader;

        private TransitionType _transitionType = TransitionType.MOVE_SCENE_LEFT;
        private Vector2 _targetPosition;
        private bool _isTransitioning = false;
        private float _transitionDuration = 2f;
        private float _transitionCount;
        private List<Vector2> _transitionPositions;

        public static Random s_random;
        public static string mapName;
        public static SaveState s_saveState;
        private Player _player;

        public World()
        {
            s_random = new Random();
            state = State.ACTIVE;
            tag = "World";
            _player = new Player();
            _tileReader = new TiledReader();
            guiManager = new GUIManager(_player);
            
            //mapName = "Room_7-7";
            mapName = "Dungeon_2-5";
            s_saveState = new SaveState();
            s_saveState.AddRoom(mapName);
            //CurrentScene = new Scene(_tileReader.LoadTiledJson("Room_7-7"), _player);
            CurrentScene = new Scene(_tileReader.LoadTiledJson("Dungeon_2-5"), _player);
         
            CurrentScene.state = State.ACTIVE;
            CurrentScene.OnPortalEnter += Scene_OnPortalEnter;
            CurrentScene.OnPlayerDead += CurrentScene_OnPlayerDead;
        }

        private void CurrentScene_OnPlayerDead(Player obj)
        {
            Portal __tempPortal = new Portal(Vector2.Zero, Vector2.Zero);
            __tempPortal.transitionType = TransitionType.BLINK;
            obj.ResetMaxHealth();
            s_saveState.rooms.Clear();
            Inventory.Instance.hasBoomerang = false;
            Inventory.Instance.hasBow = false;
            Inventory.Instance.hasCompass = false;
            Inventory.Instance.hasMap = false;
            Inventory.Instance.rupeeCount = 0;
            Inventory.Instance.keyCount = 0;
            Inventory.Instance.bombCount = 0;
            if (!IsOpenWorld())
            {
                Console.WriteLine("Dungeon");
                
                mapName = "Dungeon_2-5";
                __tempPortal.targetMap = "Dungeon_2-5";
                __tempPortal.targetPosition = new Vector2(122f, 140f);
               
                ChangeScene(new Scene(_tileReader.LoadTiledJson("Dungeon_2-5"), obj), __tempPortal);
            }
           else
            {
                Inventory.Instance.hasSword = false;
                mapName = "Room_7-7";
                __tempPortal.targetMap = "Room_7-7";
                __tempPortal.targetPosition = new Vector2(120f, 120f);
                ChangeScene(new Scene(_tileReader.LoadTiledJson("Room_7-7"), obj), __tempPortal);
                Console.WriteLine("Open");
            }
        }

        private void Scene_OnPortalEnter(Portal p_portal)
        {
            mapName = p_portal.targetMap;
            ChangeScene(new Scene(_tileReader.LoadTiledJson(p_portal.targetMap), CurrentScene.player), p_portal);
        }

        public static bool IsOpenWorld()
        {
            return mapName.StartsWith("Room");
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            if (previousScene != null && previousScene.state != State.DISABLED)
            {
                previousScene.Draw(p_spriteBatch);
            }

            if (CurrentScene.state != State.DISABLED)
            {
                CurrentScene.Draw(p_spriteBatch);
            }

            //ForegroundDraw
            if (previousScene != null && previousScene.state != State.DISABLED)
            {
                previousScene.DrawForeground(p_spriteBatch);
            }

            if (CurrentScene.state != State.DISABLED)
            {
                CurrentScene.DrawForeground(p_spriteBatch);
            }

            if (_transitionType == TransitionType.BLINK && _transitionCount <= 0.8f*_transitionDuration)
            {
                p_spriteBatch.FillRectangle(new Rectangle(0, 48 * Main.s_scale, 256 * Main.s_scale, 176 * Main.s_scale), Color.Black);
            }
                
            guiManager.Draw(p_spriteBatch);

            base.Draw(p_spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            if (previousScene != null && previousScene.state != State.DISABLED)
            {
                previousScene.DebugDraw(p_spriteBatch);
            }

            if (CurrentScene.state != State.DISABLED)
            {
                CurrentScene.DebugDraw(p_spriteBatch);
            }
            
            base.DebugDraw(p_spriteBatch);
        }

        public override void Update(float p_delta)
        {
            if (_isTransitioning)
            {
                UpdateTransition(p_delta);
            }

            if (previousScene != null && previousScene.state == State.ACTIVE)
            {
                previousScene.Update(p_delta);
            }

            if (CurrentScene.state != State.DISABLED)
            {
                CurrentScene.Update(p_delta);
            }

            guiManager.Update(p_delta);

            base.Update(p_delta);
        }

        public void OpenCloseInventory()
        {
            if (_isTransitioning) return;

            _isTransitioning = true;

            _transitionCount = 0.0f;

            _transitionPositions = new List<Vector2> { guiManager.position };

            CurrentScene.state = State.DRAW_ONLY;

            if (Main.s_gameState == Main.GameState.PLAYING)
            {
                _transitionType = TransitionType.OPEN_INVENTORY;

                Main.s_gameState = Main.GameState.INVENTORY;

                _transitionPositions.Add(Vector2.Zero);
            }
            else
            {
                _transitionType = TransitionType.CLOSE_INVENTORY;

                Main.s_gameState = Main.GameState.PLAYING;

                _transitionPositions.Add(new Vector2(0.0f, -176.0f));
            }
        }
        public void ChangeScene(Scene p_nextScene, Portal p_portal)
        {
            previousScene = CurrentScene;
            previousScene.state = State.DRAW_ONLY;

            CurrentScene = p_nextScene;
            CurrentScene.state = State.DRAW_ONLY;
            CurrentScene.player = previousScene.player;

            previousScene.RemoveEntity(previousScene.player);
            previousScene.player = null;

            _transitionType = p_portal.transitionType;
            _transitionPositions = new List<Vector2>();

            if (_transitionType == TransitionType.MOVE_SCENE_LEFT)
            {
                AddTransitionPositions(Vector2.UnitX * -256, Vector2.UnitX * 256, Vector2.UnitX * 32);
            }
            else if (_transitionType == TransitionType.MOVE_SCENE_RIGHT)
            {
                AddTransitionPositions(Vector2.UnitX * 256, Vector2.UnitX * -256, Vector2.UnitX * -32);
            }
            else if (_transitionType == TransitionType.MOVE_SCENE_DOWN)
            {
                AddTransitionPositions(Vector2.UnitY * 176, Vector2.UnitY * -176, Vector2.UnitY * -32);
            }
            else if (_transitionType == TransitionType.MOVE_SCENE_UP)
            {
                AddTransitionPositions(Vector2.UnitY * -176, Vector2.UnitY * 176, Vector2.UnitY * 32);
            }
            else if (_transitionType == TransitionType.BLINK)
            {
                AddTransitionPositions(Vector2.Zero, Vector2.Zero, Vector2.Zero);
                _targetPosition = p_portal.targetPosition;
                CurrentScene.player.ForcePosition(_targetPosition);
            }

            _isTransitioning = true;

            _transitionCount = 0.0f;
        }

        private void AddTransitionPositions(Vector2 p_previousEnd, Vector2 p_currentStart, Vector2 p_playerEnd)
        {
            _transitionPositions.Add(previousScene.scenePosition);
            _transitionPositions.Add(previousScene.scenePosition + p_previousEnd);
            _transitionPositions.Add(CurrentScene.scenePosition + p_currentStart);
            _transitionPositions.Add(CurrentScene.scenePosition);
            _transitionPositions.Add(CurrentScene.player.position - p_currentStart);
            _transitionPositions.Add(CurrentScene.player.position - p_currentStart + p_playerEnd);

            CurrentScene.scenePosition = _transitionPositions[2];
        }

        private void UpdateTransition(float p_delta)
        {
            _transitionCount += p_delta;

            if (_transitionType == TransitionType.OPEN_INVENTORY || _transitionType == TransitionType.CLOSE_INVENTORY)
            {
                _transitionCount += p_delta;
                guiManager.ForcePosition(Vector2.Lerp(_transitionPositions[0], _transitionPositions[1], _transitionCount / _transitionDuration));
            }
            else if (_transitionType != TransitionType.BLINK)
            {
                previousScene.scenePosition = Vector2.Lerp(_transitionPositions[0], _transitionPositions[1], _transitionCount / _transitionDuration);
                CurrentScene.scenePosition = Vector2.Lerp(_transitionPositions[2], _transitionPositions[3], _transitionCount / _transitionDuration);
                CurrentScene.player.ForcePosition(Vector2.Lerp(_transitionPositions[4], _transitionPositions[5], _transitionCount / _transitionDuration));
            }

            if (_transitionCount >= _transitionDuration)
            {
                if (_transitionType != TransitionType.OPEN_INVENTORY && _transitionType != TransitionType.CLOSE_INVENTORY)
                {
                    CurrentScene.scenePosition = new Vector2(0f, 48f);
                    CurrentScene.OnPortalEnter += Scene_OnPortalEnter;
                    CurrentScene.OnPlayerDead += CurrentScene_OnPlayerDead;
                    previousScene = null;
                }

                if (_transitionType != TransitionType.OPEN_INVENTORY)
                {
                    CurrentScene.state = State.ACTIVE;
                }

                _isTransitioning = false;
            }
        }
    }
}