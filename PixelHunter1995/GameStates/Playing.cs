using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.Inputs;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// State when playing. Will probably be replaced with many different states.
    /// </summary>
    class Playing : IGameState
    {
        private readonly StateManager StateManager;
        private readonly Inventory Inventory;
        private HoverText HoverText;
        private Camera camera;

        public Playing(StateManager stateManager, Inventory inventory, Camera camera)
        {
            StateManager = stateManager;
            Inventory = inventory;
            HoverText = new HoverText();
            this.camera = camera;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            spriteBatch.Begin(transformMatrix: camera.GetTransformation());
            scene.Draw(graphics, spriteBatch, 1);
            spriteBatch.End();

            spriteBatch.Begin();
            Inventory.Draw(graphics, spriteBatch, 1);
            HoverText.Draw(graphics, spriteBatch, 1);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, Scene scene, InputManager input)
        {
            if (input.GetState(InputCommand.PLAYING_Pause).IsEdgeDown)
            {
                StateManager.SetStateMenu();
            }
            
            scene.Update(gameTime, input);
            camera.Update(scene.Player.FeetPosition, scene.Width);
            HoverText.Update(input, scene.Dogs, Inventory.Items);
        }
    }
}
