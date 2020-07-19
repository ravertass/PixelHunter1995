using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// All kind of states the game can be in.
    /// </summary>
    interface IGameState
    {
        void Update(GameTime gameTime, Scene scene, Input input);
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene);
    }
}
