using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995.GameStates
{
    class StateManager
    {
        public IGameState currentState { get; internal set; }
        private SpriteBatch spriteBatch;
        private ShouldExit shouldExit;
        private Texture2D background;
        private Texture2D menu;
        private Texture2D guy;


        public StateManager (SpriteBatch spriteBatch, ShouldExit shouldExit, Texture2D background, Texture2D menu, Texture2D guy)
        {
            this.spriteBatch = spriteBatch;
            this.shouldExit = shouldExit;
            this.background = background;
            this.menu = menu;
            this.guy = guy;
        }

        public void SetExit()
        {
            shouldExit.exit = true;
        }

        public void SetStateMenu()
        {
            currentState = new Menu(this, menu, spriteBatch);
        }

        public void SetStatePlaying()
        {
            currentState = new Playing(this, background, guy, spriteBatch);
        }

    }
}
