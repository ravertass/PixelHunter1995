using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.GameStates;
using PixelHunter1995.Inputs;

namespace PixelHunter1995
{
    class GameManager
    {
        private StateManager StateManager;
        private SceneManager SceneManager;
        public GameManager()
        {
            StateManager = new StateManager();
            SceneManager = new SceneManager();
        }

        public void Initialize()
        {
            SceneManager.Initialize(Path.Combine("Content", "Scenes"));
            SceneManager.SetCurrentSceneByName("full_club_room.tmx");
        }

        void LoadContent(ContentManager content)
        {
            foreach (Scene scene in SceneManager.scenes.Values)
            {
                scene.LoadContent(content);
            }

            // Load game states
            StateManager.LoadContent(content);
            StateManager.SetStateMenu();
        }

        void Update(GameTime gameTime, InputManager input)
        {
            StateManager.currentState.Update(gameTime, SceneManager.currentScene, input);
        }

        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            StateManager.currentState.Draw(graphics, spriteBatch, gameTime, SceneManager.currentScene);
        }
    }
}
