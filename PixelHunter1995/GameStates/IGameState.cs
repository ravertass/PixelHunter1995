using Microsoft.Xna.Framework;
using System;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// All kind of states the game can be in.
    /// </summary>
    interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
