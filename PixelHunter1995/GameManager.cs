using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.GameStates;
using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;

namespace PixelHunter1995
{
    class GameManager
    {
        private Camera camera;
        private StateManager StateManager;
        private readonly SceneManager SceneManager = new SceneManager();
        private readonly Inventory Inventory = new Inventory();
        private Texture2D MenuTexture;
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void Initialize(Camera camera)
        {
            this.camera = camera;
            StateManager = new StateManager(camera);
            SceneManager.Initialize(Path.Combine("Content", "Scenes"));
            SceneManager.SetCurrentSceneByName("intro_club_room.tmx");
        }

        public void LoadContent(ContentManager content)
        {
            MenuTexture = content.Load<Texture2D>("Images/Menu");
            StateManager.SetStateMenu(MenuTexture);
            foreach (Scene scene in SceneManager.scenes.Values)
            {
                scene.LoadContent(content);
            }

            Inventory.LoadContent(content);
        }

        internal void SetExit()
        {
            ShouldExit.Instance.exit = true;
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            StateManager.currentState.Update(gameTime, input);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            StateManager.currentState.Draw(graphics, spriteBatch, gameTime);
        }

        public void StartExploring()
        {
            StateManager.SetStateExploring(Inventory, SceneManager.currentScene);
        }

        public void OpenMenu()
        {
            StateManager.SetStateMenu(MenuTexture);
        }

        public void StartDialog()
        {
            StateManager.SetStateTalking(Inventory, SceneManager.currentScene);
        }
    }
}
