using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.SceneLib;

namespace PixelHunter1995.GameStates
{
    class CursorStatus
    {
        public IDog Dog { get; private set; }
        public bool HasDog { get => Dog != null; }
        public bool LeftClicked { get; private set; }
        public bool RightClicked { get; private set; }

        public CursorStatus(Inventory inventory, Scene scene, InputManager input)
        {
            Dog = GetDogAtCursor(inventory, scene, input);
            LeftClicked = input.Input.GetKeyState(MouseKeys.LeftButton).IsEdgeDown;
            RightClicked = input.Input.GetKeyState(MouseKeys.RightButton).IsEdgeDown;
        }

        private static IDog GetDogAtCursor(Inventory inventory, Scene scene, InputManager input)
        {
            IDog dog;

            if (scene.GetDogAtCursor(input, out dog))
            {
                return dog;
            }
            inventory.GetDogAtCursor(input, out dog);

            return dog;
        }
    }
}
