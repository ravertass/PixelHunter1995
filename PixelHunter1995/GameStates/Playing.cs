using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.InventoryLib;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// State when playing. Will probably be replaced with many different states.
    /// </summary>
    class Playing : IGameState
    {
        private readonly StateManager stateManager;

        public Playing(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene, Inventory inventory)
        {
            scene.Draw(graphics, spriteBatch, 1);
            inventory.Draw(graphics, spriteBatch);
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
