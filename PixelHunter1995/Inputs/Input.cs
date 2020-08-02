using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.Inputs
{
    using AllKeys = Either<Keys, MouseKeys>;

    class Input : StateMap<AllKeys>
    {

        public Actions Actions { get; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public Point Position { get => new Point(this.X, this.Y); }

        public int ScrollWheelValue { get; private set; }
        public int HorizontalScrollWheelValue { get; private set; }

        
        public Input(Actions actions)
        {
            this.Actions = actions;
        }
        public Input(Dictionary<Action, HashSet<Dictionary<Either<Keys, MouseKeys>, SignalState>>> binds)
        {
            this.Actions = new Actions(binds);
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
            this.X = FullscreenFix.GetFixedX(mouseState.X);
            this.Y = FullscreenFix.GetFixedX(mouseState.Y);
            this.ScrollWheelValue = mouseState.ScrollWheelValue;
            this.HorizontalScrollWheelValue = mouseState.HorizontalScrollWheelValue;

            this.Actions.Update(this);
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
