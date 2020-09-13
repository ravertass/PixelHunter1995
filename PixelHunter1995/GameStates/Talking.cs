using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.DialogLib;
using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;

namespace PixelHunter1995.GameStates
{
    internal class Talking : IGameState
    {
        private readonly Inventory Inventory;
        private readonly Scene Scene;
        private readonly Camera camera;
        private DialogChoicePrompt Prompt = null;

        public Talking(Inventory inventory, Scene scene, Camera camera)
        {
            Inventory = inventory;
            Scene = scene;
            this.camera = camera;
            Prompt = new DialogChoicePrompt(new List<string>() {
                                                "What",
                                                "Could you tell me a bit more about alchemy?",
                                                "I'm selling these fine leather jackfruits.",
                                                "Excuse me, where's the hot tub?",
                                                "apa",
                                                "bepa",
                                                "Never mind."}
                                            );
        }
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: camera.GetTransformation());
            Scene.Draw(graphics, spriteBatch);
            spriteBatch.End();
            if (Prompt != null)
            {
                spriteBatch.Begin();
                Prompt.Draw(graphics, spriteBatch);
                spriteBatch.End();
            }
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            Scene.Update(gameTime, input, false);
            if (Prompt != null)
            {
                Prompt.Update(gameTime, input);
                if (!Prompt.Active)
                {
                    GameManager.Instance.StartExploring();
                }
            }
        }
    }
}
