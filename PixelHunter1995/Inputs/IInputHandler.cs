using Microsoft.Xna.Framework;

namespace PixelHunter1995.Inputs
{
    interface IInputHandler
    {

        /// <summary>
        /// Called before IUpdateable.Update to allow for handling input separately/early.
        /// Update can still be be used as well, but this gives a compartementalized option,
        /// as well as makes it possible to consume inputs with at least a single level of priority.
        /// 
        /// GameTime is supplied to allow for checking deltatime.
        /// This means keypresses that need to happen close enough in time can sum the deltatime and compare against
        /// a time-span. Using a timestamp of last input compared to current instead is problematic,
        /// as one might not want pausing to break such systems.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        void HandleInput(GameTime gameTime, Input input);

    }
}
