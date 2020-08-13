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
        private readonly Texture2D Texture;

        public Menu(Texture2D texture)
        {
            Texture = texture;
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (input.GetState(InputCommand.MENU_Exit).IsEdgeDown)
            {
                GameManager.Instance.SetExit();
            }
            // Do not trigger at same time as ToggleFullscreen
            if (input.GetState(InputCommand.MENU_Accept).IsEdgeDown
                    && input.GetState(InputCommand.ToggleFullscreen).IsUp)
            {
                GameManager.Instance.StartExploring();
            }
        }
    }
}
