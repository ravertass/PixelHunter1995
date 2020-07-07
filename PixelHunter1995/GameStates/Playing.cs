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
<<<<<<< HEAD
        private readonly Texture2D guy;
        private readonly StateManager stateManager;
        private readonly SpriteFont font;

        public Playing(StateManager stateManager, Texture2D guy, SpriteFont font)
=======
        private readonly Texture2D background;
        private readonly Player guy;
        private readonly SpriteBatch spriteBatch;
        private StateManager stateManager;

        public Playing(StateManager stateManager, Player guy, SpriteBatch spriteBatch)
>>>>>>> Basic mouse-based player movement
        {
            this.stateManager = stateManager;
            this.guy = guy;
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            scene.Draw(spriteBatch);
<<<<<<< HEAD
            spriteBatch.Draw(guy, new Vector2(220, 180), Color.White);
            spriteBatch.DrawString(font, "Hi, I'm an Alchemist! ÅÄÖ support now!", new Vector2(50, 50), Color.Blue);
=======
            guy.Draw(spriteBatch);
>>>>>>> Basic mouse-based player movement
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
