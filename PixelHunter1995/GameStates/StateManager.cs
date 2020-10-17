using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.InventoryLib;

namespace PixelHunter1995.GameStates
{
    class StateManager
    {
        public IGameState currentState { get; internal set; }
        private Camera camera;

        public StateManager()
        {
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        public void SetStateMenu(Texture2D menuTexture)
        {
            currentState = new Menu(menuTexture);
        }

        public void SetStateExploring(Inventory inventory, Scene scene)
        {
            currentState = new Exploring(inventory, scene, camera);
        }

        public void SetStateTalking(Inventory inventory, Scene scene)
        {
            currentState = new Talking(inventory, scene, camera);
        }
    }
}
