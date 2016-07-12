using System;
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

        private TransitionType _transitionType = TransitionType.MOVE_SCENE_LEFT;
        private Vector2 _targetPosition;
        private bool isTransitioning = false;
        private float _transitionDuration = 2f;
        private float _transitionCount;
        private List<Vector2> _transitionPositions;

        public static Random random;

        public World(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager)
        {
            random = new Random();
            state = State.ACTIVE;
            tag = "World";
            _tileReader = new TiledReader();
            hud = new HUD();
            CurrentScene = new Scene(_tileReader.LoadTiledJson("Dungeon_1-5"), new Player(graphicsDeviceManager));
            CurrentScene.state = State.ACTIVE;
            CurrentScene.OnPortalEnter += Scene_OnPortalEnter;
        }

        private void Scene_OnPortalEnter(Portal p_portal)
        {
            ChangeScene(new Scene(_tileReader.LoadTiledJson(p_portal.targetMap), CurrentScene.Player), p_portal);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (previousScene != null && previousScene.state != State.DISABLED)
                previousScene.Draw(spriteBatch);
            if (CurrentScene.state != State.DISABLED)
                CurrentScene.Draw(spriteBatch);

            //ForegroundDraw
            if (previousScene != null && previousScene.state != State.DISABLED)
                previousScene.DrawForeground(spriteBatch);
            if (CurrentScene.state != State.DISABLED)
                CurrentScene.DrawForeground(spriteBatch);

            if (_transitionType == TransitionType.BLINK && _transitionCount <= 0.8f * _transitionDuration)
                spriteBatch.FillRectangle(new Rectangle(0, 48 * Main.s_scale, 256 * Main.s_scale, 176 * Main.s_scale), Color.Black);

            hud.DrawHUD(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            if (previousScene != null && previousScene.state != State.DISABLED)
                previousScene.DebugDraw(p_spriteBatch);
            if (CurrentScene.state != State.DISABLED)
                CurrentScene.DebugDraw(p_spriteBatch);
            
            base.DebugDraw(p_spriteBatch);
        }
        public override void Update(float delta)
        {
            if (isTransitioning)
                UpdateTransition(delta);
            if (previousScene != null && previousScene.state == State.ACTIVE)
                previousScene.Update(delta);
            if (CurrentScene.state != State.DISABLED)
                CurrentScene.Update(delta);
            
            base.Update(delta);
            
        }

        public void ChangeScene(Scene nextScene, Portal p_portal)
        {
            previousScene = CurrentScene;
            previousScene.state = State.DRAW_ONLY;
            CurrentScene = nextScene;
            CurrentScene.state = State.DRAW_ONLY;
            CurrentScene.Player = previousScene.Player;
            previousScene.RemoveEntity(previousScene.Player);
            previousScene.Player = null;

            _transitionType = p_portal.transitionType;
            _transitionPositions = new List<Vector2>();
            if (_transitionType == TransitionType.MOVE_SCENE_LEFT)
                AddTransitionPositions(Vector2.UnitX * -256, Vector2.UnitX * 256, Vector2.UnitX * 32);
            else if (_transitionType == TransitionType.MOVE_SCENE_RIGHT)
                AddTransitionPositions(Vector2.UnitX * 256, Vector2.UnitX * -256, Vector2.UnitX * -32);
            else if (_transitionType == TransitionType.MOVE_SCENE_DOWN)
                AddTransitionPositions(Vector2.UnitY * 176, Vector2.UnitY * -176, Vector2.UnitY * -32);
            else if (_transitionType == TransitionType.MOVE_SCENE_UP)
                AddTransitionPositions(Vector2.UnitY * -176, Vector2.UnitY * 176, Vector2.UnitY * 32);
            else if (_transitionType == TransitionType.BLINK)
            {
                AddTransitionPositions(Vector2.Zero, Vector2.Zero, Vector2.Zero);
                _targetPosition = p_portal.targetPosition;
                CurrentScene.Player.ForcePosition(_targetPosition);
            }

            isTransitioning = true;
            _transitionCount = 0f;
        }
        private void AddTransitionPositions(Vector2 p_previousEnd, Vector2 p_currentStart, Vector2 p_playerEnd)
        {
            _transitionPositions.Add(previousScene.scenePosition);
            _transitionPositions.Add(previousScene.scenePosition + p_previousEnd);
            _transitionPositions.Add(CurrentScene.scenePosition + p_currentStart);
            _transitionPositions.Add(CurrentScene.scenePosition);
            _transitionPositions.Add(CurrentScene.Player.position - p_currentStart);
            _transitionPositions.Add(CurrentScene.Player.position - p_currentStart + p_playerEnd);
            CurrentScene.scenePosition = _transitionPositions[2];
        }
        private void UpdateTransition(float p_delta)
        {
            _transitionCount += p_delta;
            if (_transitionType != TransitionType.BLINK)
            {
                previousScene.scenePosition = Vector2.Lerp(_transitionPositions[0],
                    _transitionPositions[1], _transitionCount / _transitionDuration);
                CurrentScene.scenePosition = Vector2.Lerp(_transitionPositions[2],
                    _transitionPositions[3], _transitionCount / _transitionDuration);
                CurrentScene.Player.ForcePosition(Vector2.Lerp(_transitionPositions[4],
                    _transitionPositions[5], _transitionCount / _transitionDuration));
            }

            if (_transitionCount >= _transitionDuration)
            {
                CurrentScene.scenePosition = new Vector2(0f, 48f);
                CurrentScene.state = State.ACTIVE;
                CurrentScene.OnPortalEnter += Scene_OnPortalEnter;
                previousScene = null;
                isTransitioning = false;
            }
        }
    }
}
