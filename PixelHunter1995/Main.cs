﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.GameStates;
using PixelHunter1995.Components;
using PixelHunter1995.Components.Beta;
using PixelHunter1995.Components.Gamma;
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
        private Player player;
        private SoundEffect music;
        private bool musicPlaying = false;
        private SceneManager sceneManager = new SceneManager();
        private StateManager stateManager;
        private ShouldExit shouldExit;
        private RenderTarget2D renderTarget;
        private Screen screen;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
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
            renderTarget = new RenderTarget2D(GraphicsDevice, GlobalSettings.WINDOW_WIDTH, GlobalSettings.WINDOW_HEIGHT);
            screen = new Screen(graphics, Window);
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

            // Load fonts
            SpriteFont font = Content.Load<SpriteFont>("Fonts/font1");

            // Load images
            Texture2D menu = Content.Load<Texture2D>("Images/Menu");
            //Texture2D guy = Content.Load<Texture2D>("Images/snubbe");
            this.player = new PlayerGamma(this);
            player.LoadContent(Content);

            // Load sounds
            music = Content.Load<SoundEffect>("Sounds/slow-music");

            foreach (Scene scene in sceneManager.scenes.Values)
            {
                scene.LoadContent(Content);
            }

            // Load game states
            stateManager = new StateManager(spriteBatch, shouldExit, font, menu, player);
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
            screen.CheckForFullScreen();
            base.Update(gameTime);

            if (!musicPlaying)
            {
                SoundEffectInstance soundInst = music.CreateInstance();
                soundInst.Volume = 0.01f;
                soundInst.IsLooped = true;
                soundInst.Play();
                musicPlaying = true;
            }

            player.Update(gameTime);
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
            spriteBatch.Draw((Texture2D)renderTarget, screen.renderTargetRect, Color.White);
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

    }

    /// <summary>
    /// Small class to know when we should exit
    /// </summary>
    public class ShouldExit
    {
        public bool exit = false;
    }
}
