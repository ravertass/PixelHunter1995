using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public Input Input { get; }
        private readonly Inventory Inventory;

        public Playing(StateManager stateManager, Inventory inventory)
        {
            this.StateManager = stateManager;
            this.Input = new Input();
            Inventory = inventory;

        }

    public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            scene.Draw(graphics, spriteBatch, 1);
            Inventory.Draw(graphics, spriteBatch, 1);
        }

        public void HandleInput(Game game, GameTime gameTime, Input input)
        {
            this.Input.Update(game, gameTime);
            //this.Input.Hotkeys.activeActions = (HashSet<Actions>) this.Input.Hotkeys.activeActions.Concat(input.Hotkeys.activeActions);
        }

        public void Update(GameTime gameTime, Scene scene, Input input)
        {
            if (input.Hotkeys.GetState(Actions.Pause).IsEdgeDown)
            {
                StateManager.SetStateMenu();
            }

            scene.Update(gameTime, this.Input);
        }
    }
}
