using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using System.Collections.Generic;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// All kind of states the game can be in.
    /// </summary>
    interface IGameState
    {
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime);
        void Update(GameTime gameTime, InputManager input);
    }
}
