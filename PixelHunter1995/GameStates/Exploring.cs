using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.Inputs;
using PixelHunter1995.SceneLib;

namespace PixelHunter1995.GameStates
{
    class Exploring : IGameState
    {
        private readonly Inventory Inventory;
        private readonly HoverText HoverText = new HoverText();
        private readonly Scene Scene;
        private readonly Camera camera;

        public Exploring(Inventory inventory, Scene scene, Camera camera)
        {
            Inventory = inventory;
            Scene = scene;
            this.camera = camera;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: camera.GetTransformation());
            Scene.Draw(graphics, spriteBatch, 1);
            spriteBatch.End();

            spriteBatch.Begin();
            Inventory.Draw(graphics, spriteBatch, 1);
            HoverText.Draw(graphics, spriteBatch, 1);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (input.GetState(InputCommand.EXPLORING_Pause).IsEdgeDown)
            {
                GameManager.Instance.OpenMenu();
            }

            Scene.Update(gameTime, input, true);
            camera.Update(Scene.Player.FeetPosition, Scene.Width);
            HandleDogs(input);
            HandlePortals(input);
        }

        private void HandleDogs(InputManager input)
        {
            CursorStatus cursorStatus = new CursorStatus(Inventory, Scene, input);
            if (!cursorStatus.HasDog)
            {
                HoverText.UnSetText();
                return;
            }

            HoverText.SetText(cursorStatus.Dog.Name);
            if (cursorStatus.RightClicked)
            {
                GameManager.Instance.StartDialog();
            }
        }

        private void HandlePortals(InputManager input)
        {
            Portal portal;
            if (!Scene.GetPortalAtCursor(input, out portal))
            {
                return;
            }

            HoverText.SetText(portal.HoverText());

            bool leftClicked = input.Input.GetKeyState(MouseKeys.LeftButton).IsEdgeDown;
            if (leftClicked)
            {
                // TODO: Should just go to portal, and then call GoThroughPortal when player
                //       is at the portal.
                GameManager.Instance.GoThroughPortal(portal);
            }
        }
    }
}
