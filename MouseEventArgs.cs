using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoUI
{
    /// <summary>
    /// Provides a snapshot of mouse values
    /// </summary>
    public struct MouseEventArgs
    {
        /// <summary>
        /// The x-coordinate of the mouse relative to the top-left corner of the window
        /// </summary>
        public int X;
        /// <summary>
        /// The y-coordinate of the mouse relative to the top-left corner of the window
        /// </summary>
        public int Y;
        /// <summary>
        /// The state of the left mouse button relative to it's previous state
        /// </summary>
        public ButtonChangeState LeftButton;
        /// <summary>
        /// The state of the middle mouse button relative to it's previous state
        /// </summary>
        public ButtonChangeState MiddleButton;
        /// <summary>
        /// The state of the right button relative to it's previous state
        /// </summary>
        public ButtonChangeState RightButton;
        /// <summary>
        /// The position of the mouse relative to the top left corner of the window
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        /// <summary>
        /// Gets whether this mouse state is important (i.e does it hanve a mouse button change state)
        /// </summary>
        public readonly bool IsImportant;

        /// <summary>
        /// Creates a new mouse event argument
        /// </summary>
        /// <param name="x">The x position of the mouse</param>
        /// <param name="y">The y position of the mouse</param>
        /// <param name="left">The change in state of the left mouse button</param>
        /// <param name="middle">The change in state of the middle mouse button</param>
        /// <param name="right">The change in state of the right mouse button</param>
        public MouseEventArgs(int x, int y, ButtonChangeState left, ButtonChangeState middle, ButtonChangeState right)
        {
            X = x;
            Y = y;
            LeftButton = left;
            MiddleButton = middle;
            RightButton = right;
            IsImportant = (left != ButtonChangeState.None || middle != ButtonChangeState.None || RightButton != ButtonChangeState.None);
        }

        /// <summary>
        /// Creates a new mouse event argument
        /// </summary>
        /// <param name="mouseState">The current mouse state</param>
        /// <param name="prevMouseState">The previous mouse state</param>
        public MouseEventArgs(MouseState mouseState, MouseState prevMouseState)
        {
            if (mouseState.LeftButton != prevMouseState.LeftButton ||
            mouseState.MiddleButton != prevMouseState.MiddleButton ||
            mouseState.RightButton != prevMouseState.RightButton)
            {
                X = mouseState.X;
                Y = mouseState.Y;
                LeftButton = (mouseState.LeftButton == ButtonState.Released & prevMouseState.LeftButton == ButtonState.Pressed) ?
                ButtonChangeState.Released :
                (mouseState.LeftButton == ButtonState.Pressed & prevMouseState.LeftButton == ButtonState.Released) ?
                ButtonChangeState.Pressed : ButtonChangeState.None;

                MiddleButton = (mouseState.MiddleButton == ButtonState.Released & prevMouseState.MiddleButton == ButtonState.Pressed) ?
                  ButtonChangeState.Released :
                  (mouseState.MiddleButton == ButtonState.Pressed & prevMouseState.MiddleButton == ButtonState.Released) ?
                  ButtonChangeState.Pressed : ButtonChangeState.None;

                RightButton = (mouseState.RightButton == ButtonState.Released & prevMouseState.RightButton == ButtonState.Pressed) ?
                ButtonChangeState.Released :
                (mouseState.RightButton == ButtonState.Pressed & prevMouseState.RightButton == ButtonState.Released) ?
                ButtonChangeState.Pressed : ButtonChangeState.None;

                IsImportant = true;
            }

            else if (mouseState.LeftButton == ButtonState.Pressed & prevMouseState.LeftButton == ButtonState.Pressed ||
                mouseState.RightButton == ButtonState.Pressed & prevMouseState.RightButton == ButtonState.Pressed ||
                prevMouseState.MiddleButton == ButtonState.Pressed & prevMouseState.MiddleButton == ButtonState.Pressed)
            {
                X = mouseState.X;
                Y = mouseState.Y;
                LeftButton = (mouseState.LeftButton == ButtonState.Released & prevMouseState.LeftButton == ButtonState.Pressed) ?
                ButtonChangeState.Released :
                (mouseState.LeftButton == ButtonState.Pressed) ?
                ButtonChangeState.Pressed : ButtonChangeState.None;

                MiddleButton = (mouseState.MiddleButton == ButtonState.Released & prevMouseState.MiddleButton == ButtonState.Pressed) ?
                ButtonChangeState.Released :
                (mouseState.MiddleButton == ButtonState.Pressed) ?
                ButtonChangeState.Pressed : ButtonChangeState.None;

                RightButton = (mouseState.RightButton == ButtonState.Released & prevMouseState.RightButton == ButtonState.Pressed) ?
                ButtonChangeState.Released :
                (mouseState.RightButton == ButtonState.Pressed) ?
                ButtonChangeState.Pressed : ButtonChangeState.None;

                IsImportant = false;
            }
            else
            {
                X = mouseState.X;
                Y = mouseState.Y;
                LeftButton = ButtonChangeState.None;
                RightButton = ButtonChangeState.None;
                MiddleButton = ButtonChangeState.None;
                IsImportant = false;
            }
        }
    }
}
