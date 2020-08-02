using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.Inputs;
using System.Collections.Generic;

namespace PixelHunter1995.GameStates
{
    /// <summary>
    /// State when playing. Will probably be replaced with many different states.
    /// </summary>
    class Playing : IGameState
    {
        private readonly StateManager StateManager;
        // public Input Input { get; }
        private readonly Inventory Inventory;

        public Playing(StateManager stateManager, Inventory inventory)
        {
            this.StateManager = stateManager;
            // this.Input = new Input();
            Inventory = inventory;

        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            scene.Draw(graphics, spriteBatch, 1);
            Inventory.Draw(graphics, spriteBatch, 1);
        }

        public void Update(GameTime gameTime, Scene scene, Input input)
        {
            if (input.Actions.GetState(Action.Pause).IsEdgeDown)
            {
                StateManager.SetStateMenu();
            }

            scene.HandleInput(gameTime, input);
            scene.Update(gameTime, input);
        }
    }
}
