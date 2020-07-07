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
        private readonly Texture2D guy;
        private StateManager stateManager;

        public Playing(StateManager stateManager, Texture2D guy)
        {
            this.stateManager = stateManager;
            this.guy = guy;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            scene.Draw(spriteBatch);
            spriteBatch.Draw(guy, new Vector2(20,20), Color.White);
        }

        public void Update(GameTime gameTime, Scene scene)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                stateManager.SetStateMenu();
            }
        }
    }
}
