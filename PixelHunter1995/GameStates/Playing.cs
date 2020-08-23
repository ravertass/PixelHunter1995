﻿using Microsoft.Xna.Framework;
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

        public Playing(StateManager stateManager, Inventory inventory)
        {
            StateManager = stateManager;
            Inventory = inventory;
            HoverText = new HoverText();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime, Scene scene)
        {
            scene.Draw(graphics, spriteBatch, 1);
            Inventory.Draw(graphics, spriteBatch, 1);
            HoverText.Draw(graphics, spriteBatch, 1);
        }

        public void Update(GameTime gameTime, Scene scene, InputManager input)
        {
            if (input.GetState(InputCommand.PLAYING_Pause).IsEdgeDown)
            {
                StateManager.SetStateMenu();
            }
            
            scene.Update(gameTime, input);
            Inventory.Update(gameTime, input);
            HoverText.Update(input, scene.Dogs, Inventory.Items);
        }
    }
}
