using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;

    class InputReader
    {

        public StateMap<AllKeys> Statemap { get; }

        public int MouseX { get; private set; }
        public int MouseY { get; private set; }
        public Point Position { get => new Point(this.MouseX, this.MouseY); }

        public int ScrollWheelValue { get; private set; }
        // public int HorizontalScrollWheelValue { get; private set; }

        public InputReader()
        {
            this.Statemap = new StateMap<AllKeys>();
        }


        /// <summary>
        /// Called to update the state of inputs. Perhaps even more often than per-frame? Event-based?
        /// </summary>
        public void Update()
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            // Handle mouse
            var pressedMouse = new Dictionary<MouseKeys, ButtonState>()
            {
                [MouseKeys.LeftButton] = mouseState.LeftButton,
                [MouseKeys.RightButton] = mouseState.RightButton,
                [MouseKeys.MiddleButton] = mouseState.MiddleButton,
                [MouseKeys.XButton1] = mouseState.XButton1,
                [MouseKeys.XButton2] = mouseState.XButton2,
            }.Where(kv => kv.Value == ButtonState.Pressed).Select(kv => (AllKeys) kv.Key);

            // Merge keyboard and mouse
            var pressedKeys = pressedMouse.Concat(keyboardState.GetPressedKeys().Select(k => (AllKeys) k));
           
            this.Update(pressedKeys);
            
            // mouse position
            this.MouseX = Screen.GetFixedX(mouseState.X);
            this.MouseY = Screen.GetFixedY(mouseState.Y);
            this.ScrollWheelValue = mouseState.ScrollWheelValue;
            // this.HorizontalScrollWheelValue = mouseState.HorizontalScrollWheelValue;
        }

        private void Update(IEnumerable<AllKeys> pressedKeys)
        {
            this.Statemap.Update(pressedKeys);
        }

        public SignalState GetKeyState(AllKeys key)
        {
            return this.Statemap.GetState(key);
        }
    }

    /// <summary>
    /// An enum for the various keys on a mouse,
    /// as well as `None` for when one wants to make it explicitly that it relates to none.
    /// </summary>
    enum MouseKeys
    {
        None,
        LeftButton,
        RightButton,
        MiddleButton,
        XButton1,
        XButton2
    }
}
