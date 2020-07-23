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

        public Input Input { get; }

        public Menu(StateManager stateManager, Texture2D menu)
        {
            this.stateManager = stateManager;
            this.menu = menu;
            this.Input = new Input();
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            spriteBatch.Draw(menu, Vector2.Zero, Color.White);
        }

        public void Update(GameTime gameTime, Scene scene, Input input)
        {
            if (input.Hotkeys.GetState(Actions.Exit).IsEdgeDown)
            {
                stateManager.SetExit();
            }
            else if (input.Hotkeys.GetState(Actions.Accept).IsEdgeDown)
            {
                stateManager.SetStatePlaying();
            }
        }
    }
}
