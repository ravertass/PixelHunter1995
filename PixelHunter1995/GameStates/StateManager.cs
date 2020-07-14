﻿using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995.GameStates
{
    class StateManager
    {
        public IGameState currentState { get; internal set; }
        private SpriteFont font;
        private ShouldExit shouldExit;
        private Texture2D menu;


        public StateManager (ShouldExit shouldExit, SpriteFont font, Texture2D menu)
        {
            this.shouldExit = shouldExit;
            this.menu = menu;
            this.font = font;
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
            currentState = new Playing(this, font);
        }

    }
}
