using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.GameStates;
using System.Collections.Generic;
using System.IO;

namespace PixelHunter1995
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SoundEffect music;
        private bool musicPlaying = false;
        private SceneManager sceneManager = new SceneManager();
        private bool justToggledFullscreen = false;
        private StateManager stateManager;
        private ShouldExit shouldExit;
        private RenderTarget2D renderTarget;
        private Rectangle renderTargetRect;
        public const int WINDOW_WIDTH = 426;
        public const int WINDOW_HEIGHT = 240;

        private int fullScreenWidth;
        private int fullScreenHeight;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            sceneManager.Initialize(Path.Combine("Content", "Scenes"));
            sceneManager.SetCurrentSceneByName("club_room.tmx");
            GlobalSettings.Instance.Debug = true;
            shouldExit = new ShouldExit();
            renderTarget = new RenderTarget2D(GraphicsDevice, WINDOW_WIDTH, WINDOW_HEIGHT);
            SetFullScreenResolution();
            SetToWindowed();
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            // Load images
            Texture2D menu = Content.Load<Texture2D>("Images/Menu");
            Texture2D guy = Content.Load<Texture2D>("Images/snubbe");

            // Load sounds
            music = Content.Load<SoundEffect>("Sounds/slow-music");

            foreach (Scene scene in sceneManager.scenes.Values)
            {
                scene.LoadContent(Content);
            }

            // Load game states
            stateManager = new StateManager(shouldExit, menu, guy);
            stateManager.SetStateMenu();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        private bool IsAltEnterPressed(KeyboardState state)
        {
            return (state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt)) && state.IsKeyDown(Keys.Enter);
        }

        private void CheckForFullScreen()
        {
            KeyboardState state = Keyboard.GetState();
            if (!justToggledFullscreen && IsAltEnterPressed(state))
            {
                ToggleFullScreen();
                justToggledFullscreen = true;
            }

            if (!IsAltEnterPressed(state))
            {
                justToggledFullscreen = false;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (shouldExit.exit)
            {
                Exit();
            }

            stateManager.currentState.Update(gameTime, sceneManager.currentScene);
            CheckForFullScreen();
            base.Update(gameTime);

            if (!musicPlaying)
            {
                SoundEffectInstance soundInst = music.CreateInstance();
                soundInst.Volume = 0.5f;
                soundInst.IsLooped = true;
                soundInst.Play();
                musicPlaying = true;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawToRenderTarget(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw((Texture2D)renderTarget, renderTargetRect, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawToRenderTarget(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            stateManager.currentState.Draw(spriteBatch, gameTime, sceneManager.currentScene);
            spriteBatch.End();

            sceneManager.currentScene.walkingArea.Draw(graphics);

            GraphicsDevice.SetRenderTarget(null);
        }

        public void ToggleFullScreen()
        {
            if (!Window.IsBorderless)
                SetToFullScreen();
            else
                SetToWindowed();
        }

        private void SetToFullScreen()
        {
            renderTargetRect = RenderTargetFullScreenRect();
            graphics.PreferredBackBufferWidth = fullScreenWidth;
            graphics.PreferredBackBufferHeight = fullScreenHeight;
            graphics.ApplyChanges();
            Window.IsBorderless = true;
            Window.Position = new Point(0, 0);

        }

        private void SetToWindowed()
        {
            renderTargetRect = RenderTargetWindowRect();
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();
            Window.IsBorderless = false;
            Window.Position = new Point((fullScreenWidth - WINDOW_WIDTH) / 2,
                                        (fullScreenHeight - WINDOW_HEIGHT) / 2);

        }

        private Rectangle RenderTargetFullScreenRect()
        {
            if (fullScreenWidth > fullScreenHeight)
            {
                int newWindowWidth = (int)System.Math.Ceiling(
                    ((float)fullScreenHeight / (float)WINDOW_HEIGHT) * WINDOW_WIDTH);

                return new Rectangle((fullScreenWidth - newWindowWidth) / 2,
                    0, newWindowWidth, fullScreenHeight);
            }
            else
            {
                int newWindowHeight = (int)System.Math.Ceiling(
                    ((float)fullScreenWidth/ (float)WINDOW_WIDTH) * WINDOW_HEIGHT);

                return new Rectangle(0, (fullScreenHeight - newWindowHeight) / 2,
                    fullScreenWidth, newWindowHeight);
            }
        }

        private Rectangle RenderTargetWindowRect()
        {
            return new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
        }

        private void SetFullScreenResolution()
        {
            fullScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            fullScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }


    }



    /// <summary>
    /// Small class to know when we should exit
    /// </summary>
    public class ShouldExit
    {
        public bool exit = false;
    }
}
