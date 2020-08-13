using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.Inputs;
using System.Collections.Generic;
using System.Linq;
using PixelHunter1995.SceneLib;
using PixelHunter1995.Utilities;

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
            if (input.GetState(InputCommand.PLAYING_Pause).IsEdgeDown)
            {
                GameManager.Instance.OpenMenu();
            }

            Scene.Update(gameTime, input, true);
            camera.Update(Scene.Player.FeetPosition, Scene.Width);
            List<IDog> allDogs = Scene.Dogs.Concat(Inventory.Items).ToList();
            CursorStatus cursorStatus = new CursorStatus(allDogs, input);
            if (cursorStatus.Dog == null)
            {
                HoverText.UnSetText();
            }
            else
            {
                HoverText.SetText(cursorStatus.Dog.Name);
                if (cursorStatus.RightClicked)
                {
                    GameManager.Instance.StartDialog();
                }
            }
        }

        private class CursorStatus
        {
            public IDog Dog = null;
            public bool LeftClicked = false;
            public bool RightClicked = false;
            public CursorStatus(List<IDog> dogs, InputManager input)
            {
                Coord mousePos = new Coord(input.MouseX, input.MouseY);
                // We sort on Z index, to check the top dog first. Note that this is reversed from
                // when we draw them, since in that case we want to draw the thing on top last.
                dogs.Sort((a, b) => b.ZIndex().CompareTo(a.ZIndex()));
                Dog = null;
                foreach (IDog dog in dogs)
                {
                    if (dog.Contains(mousePos))
                    {
                        Dog = dog;
                        break;
                    }
                }
                LeftClicked = input.Input.GetKeyState(MouseKeys.LeftButton).IsEdgeDown;
                RightClicked = input.Input.GetKeyState(MouseKeys.RightButton).IsEdgeDown;
            }
        }
    }
}
