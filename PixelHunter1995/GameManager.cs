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
        public readonly MusicManager MusicManager = new MusicManager();
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

            MusicManager.LoadContent(content);
        }

        internal void SetExit()
        {
            ShouldExit.Instance.exit = true;
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            StateManager.currentState.Update(gameTime, input);
            MusicManager.Update();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            StateManager.currentState.Draw(graphics, spriteBatch, gameTime);
        }

        public void StartExploring()
        {
            // TODO: Put this somewhere else. But not in Initialize(), since LoadContent() isn't called before?
            // TODO: Song should be based on a property in the scene.
            MusicManager.StartSong("club_room");
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

        public void GoThroughPortal(Portal portal)
        {
            StateManager.SetStateChangeScene(Inventory, SceneManager.currentScene, portal);
        }

        public void ChangeScene(string sceneName, string portalName)
        {
            SceneManager.SetCurrentSceneByName(sceneName);
            StateManager.SetStateExploring(Inventory, SceneManager.currentScene);
            Portal destinationPortal = SceneManager.currentScene.GetPortalByName(portalName);
            SceneManager.currentScene.Player.SetPosition(destinationPortal.AppearancePosition, destinationPortal.ReverseExitDirection());
            camera.GoTo((int)destinationPortal.AppearancePosition.X, SceneManager.currentScene.Width);
        }
    }
}
