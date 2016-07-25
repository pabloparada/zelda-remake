using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda
{
    public class InputManager
    {
        public enum MouseButtons
        {
            LEFT_BUTTON,
            RIGHT_BUTTON,
            MIDDLE_BUTTON
        }

        private static readonly Dictionary<Direction, Tuple<Direction, Vector2>> _directionCache = new Dictionary<Direction, Tuple<Direction, Vector2>>
        {
            { Direction.UP, new Tuple<Direction, Vector2>(Direction.UP, new Vector2(0, -1)) },
            { Direction.LEFT, new Tuple<Direction, Vector2>(Direction.LEFT, new Vector2(-1, 0)) },
            { Direction.RIGHT, new Tuple<Direction, Vector2>(Direction.RIGHT, new Vector2(1, 0)) },
            { Direction.DOWN, new Tuple<Direction, Vector2>(Direction.DOWN, new Vector2(0, 1)) },
            { Direction.NONE, new Tuple<Direction, Vector2>(Direction.NONE, new Vector2(0, 0)) }
        };

        private static Tuple<Direction, Vector2> _previousDirectionCache;

        private static MouseState s_oldMouseState;
        public static MouseState s_mouseState { get; private set; }

        private static KeyboardState s_oldKeyboardState;
        public static KeyboardState s_keyboardState { get; private set; }

        public InputManager()
        {
            UpdateOldState();
            UpdateState();
        }
        public void UpdateOldState()
        {
            s_oldKeyboardState = Keyboard.GetState();
        }
        public void UpdateState()
        {
            s_keyboardState = Keyboard.GetState();
        }

        public static bool GetKeyChange(Keys p_key)
        {
            if (s_oldKeyboardState.IsKeyDown(p_key) && s_keyboardState.IsKeyUp(p_key))
            {
                return true;
            }

            return false;
        }

        public static Tuple<Direction, Vector2> GetDirection()
        {
            if (s_keyboardState.IsKeyDown(Keys.W)) return _directionCache[Direction.UP];
            else if (s_keyboardState.IsKeyDown(Keys.A)) return _directionCache[Direction.LEFT];
            else if (s_keyboardState.IsKeyDown(Keys.D)) return _directionCache[Direction.RIGHT];
            else if (s_keyboardState.IsKeyDown(Keys.S)) return _directionCache[Direction.DOWN];
            else return _directionCache[Direction.NONE];
        }

        public static Vector2 GetDirectionVectorByDirectionEnum(Direction p_direction)
        {
            return _directionCache[p_direction].Item2;
        }

        public static Direction GetDirectionEnumByDirectionVector(Vector2 p_direction)
        {
            if (p_direction == Vector2.UnitX) return Direction.RIGHT;
            else if (p_direction == Vector2.UnitY) return Direction.DOWN;
            else if (p_direction == -Vector2.UnitY) return Direction.UP;
            else if (p_direction == -Vector2.UnitX) return Direction.LEFT;
            else return Direction.NONE;
        }

        public static string GetAnimationNameByDirection(Direction p_direction)
        {
            var __name = p_direction.ToString();

            return char.ToUpper(__name[0]) + __name.Substring(1).ToLower();
        }
    }
}
