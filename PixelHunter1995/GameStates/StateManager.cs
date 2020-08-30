﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.InventoryLib;

namespace PixelHunter1995.GameStates
{
    class StateManager : ILoadContent
    {
        public IGameState currentState { get; internal set; }
        private readonly Inventory Inventory = new Inventory();
        private Texture2D Menu;
        private Camera camera;

        public StateManager(Camera camera)
        {
            this.camera = camera;
        }

        // We need to save content here since we create new versions of the states each time.
        public void LoadContent(ContentManager content)
        {
            Inventory.LoadContent(content);
            Menu = content.Load<Texture2D>("Images/Menu");
        }

        public void SetExit()
        {
            ShouldExit.Instance.exit = true;
        }

        public void SetStateMenu()
        {
            currentState = new Menu(this, Menu);
        }

        public void SetStatePlaying()
        {
            currentState = new Playing(this, Inventory, camera);
        }
    }
}
