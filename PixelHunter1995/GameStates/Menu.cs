using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// State when being in main menu
    /// </summary>
    class Menu : IGameState
    {
        private readonly Texture2D menu;
        private StateManager stateManager;

        public Menu(StateManager stateManager, Texture2D menu)
        {
            this.stateManager = stateManager;
            this.menu = menu;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            spriteBatch.Draw(menu, Vector2.Zero, Color.White);
        }

        public void Update(GameTime gameTime, Scene scene, Input input)
        {
            if (input.GetKeyState(Keys.Escape).IsEdgeDown)
            {
                stateManager.SetExit();
            }
            else if (input.GetKeyState(Keys.Enter).IsEdgeDown)
            {
                stateManager.SetStatePlaying();
            }
        }
    }
}
