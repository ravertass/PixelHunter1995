using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.GameStates;
using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.SceneLib;

namespace PixelHunter1995
{
    class GameManager
    {
        private Camera camera;
        private StateManager StateManager;
        public readonly SceneManager SceneManager = new SceneManager();
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
            SceneManager.SetCurrentSceneByName("club_room");
            camera.GoTo(
                (int)SceneManager.currentScene.Player.Position.X, SceneManager.currentScene.Width);
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

        public void GoToPortal(string sceneName, string portalName)
        {
            // TODO: Unsure where this logic should go...
            // TODO: Should probably first change state to a transition state where the player
            //       walks through the portal, then change room when the transition state is done.
            SceneManager.SetCurrentSceneByName(sceneName);
            StateManager.SetStateExploring(Inventory, SceneManager.currentScene);
            Portal destinationPortal = SceneManager.currentScene.GetPortalByName(portalName);
            SceneManager.currentScene.Player.SetPosition(destinationPortal.AppearancePosition);
            camera.GoTo((int)destinationPortal.AppearancePosition.X, SceneManager.currentScene.Width);
        }
    }
}
