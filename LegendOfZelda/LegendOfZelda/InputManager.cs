using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TicTacToe
{
    class InputManager
    {
        public enum MouseButtons
        {
            LEFT_BUTTON,
            RIGHT_BUTTON,
            MIDDLE_BUTTON
        }
        static MouseState s_oldMouseState;
        public static MouseState s_mouseState;

        static KeyboardState s_oldKeyboardState;
        public static KeyboardState s_keyboardState;
        public void UpdateOldState()
        {
            s_oldMouseState = Mouse.GetState();
        }
        public void UpdateState()
        {
            s_mouseState = Mouse.GetState();
        }

        public static bool GetMouseButtonChange(MouseButtons p_button, bool p_pressed)
        {
            
            ButtonState __buttonStateOld;
            ButtonState __buttonState;
            if (p_button == MouseButtons.LEFT_BUTTON)
            {
                __buttonStateOld = s_oldMouseState.LeftButton;
                __buttonState = s_mouseState.LeftButton;
            }
            else if (p_button == MouseButtons.RIGHT_BUTTON)
            {
                __buttonStateOld = s_oldMouseState.RightButton;
                __buttonState = s_mouseState.RightButton;
            }
            else
            {
                __buttonStateOld = s_oldMouseState.MiddleButton;
                __buttonState = s_mouseState.MiddleButton;
            }
            if (p_pressed && __buttonState != __buttonStateOld && __buttonState == ButtonState.Pressed)
                return true;
            else if (!p_pressed && __buttonState != __buttonStateOld && __buttonState == ButtonState.Released)
                return true;
            return false;
        }
        public static bool GetKeyChange(Keys p_key, bool p_pressed)
        {
            bool __keyState = s_keyboardState.IsKeyDown(p_key);
            if (__keyState != s_oldKeyboardState.IsKeyDown(p_key) && __keyState)
                return true;
            return false;
        }
    }
}
