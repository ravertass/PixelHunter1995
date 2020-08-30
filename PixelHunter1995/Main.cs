using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PixelHunter1995.Utilities;
using PixelHunter1995.Inputs;

namespace PixelHunter1995
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        GameManager GameManager;
        SpriteBatch spriteBatch;
        private SoundEffect music;
        private bool musicPlaying = false;
        private RenderTarget2D renderTarget;
        private Screen screen;
        private InputManager input;
        private Camera camera;
        
        private static readonly string inputConfigPath = "Content/Config/input.cfg";

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
            camera = new Camera();
            GameManager = new GameManager(camera);
            GameManager.Initialize();
            GlobalSettings.Instance.Debug = true;
            renderTarget = new RenderTarget2D(GraphicsDevice, GlobalSettings.WINDOW_WIDTH, GlobalSettings.WINDOW_HEIGHT);
            screen = new Screen(graphics, Window, camera);
            Screen.Instance = screen;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            
            this.input = InputConfigParser.ParseInputConfig(inputConfigPath)
                    [InputConfigParser.DEFAULT_CONTEXT];

            // Load sounds
            music = Content.Load<SoundEffect>("Sounds/Hallways");

            GameManager.LoadContent(Content);

            // Load fonts
            FontManager.Instance.LoadContent(Content);
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
            if (ShouldExit.Instance.exit)
            {
                Exit();
            }

            // The game requires focus to handle input.
            if (this.IsActive)
            {
                this.input.Update(); // handle input for global actions
                
                
                if (input.GetState(InputCommand.ToggleFullscreen).IsEdgeDown)
                {
                    screen.ToggleFullScreen();
                }
            }

            GameManager.Update(gameTime, input);
            base.Update(gameTime);

            if (!musicPlaying)
            {
                SoundEffectInstance soundInst = music.CreateInstance();
                soundInst.Volume = 0.1f;
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
            // SamplerState.PointClamp is needed to skip smoothing in fullscreen mode. The others are just
            // the default values.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw((Texture2D)renderTarget, screen.renderTargetRect, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawToRenderTarget(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            GameManager.Draw(graphics, spriteBatch, gameTime);

            GraphicsDevice.SetRenderTarget(null);
        }

    }

    /// <summary>
    /// Small class to know when we should exit
    /// </summary>
    public class ShouldExit
    {
        private static ShouldExit instance;

        public static ShouldExit Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShouldExit();
                }
                return instance;
            }
        }

        public bool exit = false;
    }
}
