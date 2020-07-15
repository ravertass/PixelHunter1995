using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// State when playing. Will probably be replaced with many different states.
    /// </summary>
    class Playing : IGameState
    {
        private readonly StateManager stateManager;
        private readonly SpriteFont font;

        public Playing(StateManager stateManager, SpriteFont font)
        {
            this.stateManager = stateManager;
            this.font = font;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            scene.Draw(graphics, spriteBatch);
            spriteBatch.DrawString(font, "Hi, I'm an Alchemist! ÅÄÖ support now!", new Vector2(50, 50), Color.Blue);
        }

        public void Update(GameTime gameTime, Scene scene)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                stateManager.SetStateMenu();
            }

            scene.Update(gameTime);
        }
    }
}
