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
        private readonly Texture2D background;
        private readonly Texture2D guy;
        private readonly SpriteBatch spriteBatch;
        private StateManager stateManager;

        public Playing(StateManager stateManager, Texture2D background, Texture2D guy, SpriteBatch spriteBatch)
        {
            this.stateManager = stateManager;
            this.background = background;
            this.guy = guy;
            this.spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(guy, new Vector2(20,20), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                stateManager.SetStateMenu();
            }
        }
    }
}
