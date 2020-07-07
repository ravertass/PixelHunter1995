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
        private SceneHandler sceneHandler = new SceneHandler();
        private bool justToggledFullscreen = false;
        private StateManager stateManager;
        private ShouldExit shouldExit;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 426;
            graphics.PreferredBackBufferHeight = 240;
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
            // TODO: Add your initialization logic here
            sceneHandler.Initialize(Path.Combine("Content", "Scenes"));
            sceneHandler.SetCurrentSceneByName("scene1.tmx");
            GlobalSettings.Instance.Debug = true;
            shouldExit = new ShouldExit();
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
            foreach (Scene scene in sceneHandler.scenes.Values)
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
                graphics.ToggleFullScreen();
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

            stateManager.currentState.Update(gameTime, sceneHandler.currentScene);
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
            spriteBatch.Begin();
            stateManager.currentState.Draw(spriteBatch, gameTime, sceneHandler.currentScene);
            spriteBatch.End();
            sceneHandler.currentScene.walkingArea.Draw(graphics);
            base.Draw(gameTime);
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
