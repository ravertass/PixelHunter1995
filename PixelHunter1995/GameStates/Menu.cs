using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;

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
            if (input.Actions.GetState(Action.MENU_Exit).IsEdgeDown)
            {
                stateManager.SetExit();
            }
            // Do not trigger at same time as ToggleFullscreen
            if (input.Actions.GetState(Action.MENU_Accept).IsEdgeDown
                    && input.Actions.GetState(Inputs.Action.ToggleFullscreen).IsUp)
            {
                stateManager.SetStatePlaying();
            }
        }
    }
}
