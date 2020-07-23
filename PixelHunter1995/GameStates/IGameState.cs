using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// All kind of states the game can be in.
    /// </summary>
    interface IGameState
    {
        /// <summary>
        /// States has their own input instance, to ensure the edges work correctly as they are enabled/disabled.
        /// ie. if player holds a key while pausing and then release it,
        /// there still needs to be an EdgeUp when resuming. Otherwise, one can achieve locked input/keys.
        /// </summary>
        Input Input { get; }

        void HandleInput(Game game, GameTime gameTime, Input input);
        void Update(GameTime gameTime, Scene scene, Input input);
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene);
    }
}
