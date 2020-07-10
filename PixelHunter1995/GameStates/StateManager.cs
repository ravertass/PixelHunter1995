using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Components.Gamma;

namespace PixelHunter1995.GameStates
{
    class StateManager
    {
        public IGameState currentState { get; internal set; }
        private SpriteFont font;
        private ShouldExit shouldExit;
        private Texture2D menu;
        SpriteBatch spriteBatch;

        private IDrawable playerDrawable;
        private IUpdateable playerUpdateable;

        public StateManager (SpriteBatch spriteBatch, ShouldExit shouldExit, SpriteFont font, Texture2D menu, IDrawable playerDrawable, IUpdateable playerUpdateable)
        {
            this.spriteBatch = spriteBatch;
            this.shouldExit = shouldExit;
            this.menu = menu;
            this.font = font;
            this.playerDrawable = playerDrawable;
            this.playerUpdateable = playerUpdateable;
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
            currentState = new Playing(this, playerDrawable, playerUpdateable, font, spriteBatch);
        }

    }
}
