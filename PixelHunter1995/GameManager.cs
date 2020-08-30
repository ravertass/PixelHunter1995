using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.GameStates;
using PixelHunter1995.Inputs;

namespace PixelHunter1995
{
    class GameManager
    {
        private Camera camera;
        private SceneManager SceneManager;
        private StateManager StateManager;

        public GameManager(Camera camera)
        {
            this.camera = camera;
            SceneManager = new SceneManager();
            StateManager = new StateManager(camera);
        }

        public void Initialize()
        {
            SceneManager.Initialize(Path.Combine("Content", "Scenes"));
            SceneManager.SetCurrentSceneByName("full_club_room.tmx");
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Scene scene in SceneManager.scenes.Values)
            {
                scene.LoadContent(content);
            }

            // Load game states
            StateManager.LoadContent(content);
            StateManager.SetStateMenu();
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            StateManager.currentState.Update(gameTime, SceneManager.currentScene, input);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            StateManager.currentState.Draw(graphics, spriteBatch, gameTime, SceneManager.currentScene);
        }
    }
}
