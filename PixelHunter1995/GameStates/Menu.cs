using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.InventoryLib;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// State when being in main menu
    /// </summary>
    class Menu : IGameState
    {
        private readonly Texture2D menu;
        private StateManager stateManager;
        private bool escapeHasBeenUp;

        public Menu(StateManager stateManager, Texture2D menu)
        {
            this.stateManager = stateManager;
            this.menu = menu;
            this.escapeHasBeenUp = false;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene, Inventory inventory)
        {
            spriteBatch.Draw(menu, Vector2.Zero, Color.White);
        }

        public void Update(GameTime gameTime, Scene scene)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyUp(Keys.Escape))
            {
                escapeHasBeenUp = true;
            }

            if (state.IsKeyDown(Keys.Escape) && escapeHasBeenUp)
            {
                stateManager.SetExit();
            }
            else if (state.IsKeyDown(Keys.Enter))
            {
                escapeHasBeenUp = false;
                stateManager.SetStatePlaying();
            }
        }
    }
}
