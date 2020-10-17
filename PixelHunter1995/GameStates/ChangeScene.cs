using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.SceneLib;

namespace PixelHunter1995.GameStates
{
    class ChangeScene : IGameState
    {
        private readonly Inventory Inventory;
        private readonly Scene CurrentScene;
        private readonly string NextSceneName;
        private readonly string NextPortalName;
        private readonly Camera Camera;
        private readonly Vector2 EndPosition;
        private readonly ExitDirection ExitDirection;

        public ChangeScene(Inventory inventory,
                           Scene currentScene,
                           Camera camera,
                           Portal portal)
        {
            Inventory = inventory;
            CurrentScene = currentScene;
            Camera = camera;
            NextSceneName = portal.DestinationScene;
            NextPortalName = portal.DestinationPortal;
            EndPosition = portal.ExitPosition;
            ExitDirection = portal.ExitDirection;

            Player player = CurrentScene.Player;
            player.MovePosition = EndPosition;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.GetTransformation());
            CurrentScene.Draw(graphics, spriteBatch, 1);
            spriteBatch.End();

            spriteBatch.Begin();
            Inventory.Draw(graphics, spriteBatch, 1);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (input.GetState(InputCommand.EXPLORING_Pause).IsEdgeDown)
            {
                GameManager.Instance.OpenMenu();
            }

            CurrentScene.Update(gameTime, input, false);
            Camera.Update(CurrentScene.Player.FeetPosition, CurrentScene.Width);

            if (CurrentScene.Player.Position == EndPosition)
            {
                GameManager.Instance.ChangeScene(NextSceneName, NextPortalName);
            }
        }
    }
}
