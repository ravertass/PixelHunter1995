using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995.GameStates
{
    class StateManager
    {
        public IGameState currentState { get; internal set; }
        private ShouldExit shouldExit;
        private Texture2D menu;
        private Texture2D guy;


        public StateManager (ShouldExit shouldExit, Texture2D menu, Texture2D guy)
        {
            this.shouldExit = shouldExit;
            this.menu = menu;
            this.guy = guy;
        }

        public void SetExit()
        {
            shouldExit.exit = true;
        }

        public void SetStateMenu()
        {
            currentState = new Menu(this, menu);
        }

        public void SetStatePlaying()
        {
            currentState = new Playing(this, guy);
        }

    }
}
