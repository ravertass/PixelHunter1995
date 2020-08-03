using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Utilities;
using PixelHunter1995.Inputs;

namespace PixelHunter1995
{
    /** This class is responsible for switching between windowed and fullscreen mode.
     */
    internal class Screen
    {

        public Rectangle renderTargetRect;
        public int Width { get; private set; }
        public int Height { get; private set; }

        internal int fullScreenWidth;
        internal int fullScreenHeight;

        private readonly GraphicsDeviceManager graphics;
        private readonly GameWindow window;

        public Screen(GraphicsDeviceManager graphics, GameWindow window)
        {
            this.graphics = graphics;
            this.window = window;
            this.Width = GlobalSettings.WINDOW_WIDTH;
            this.Height = GlobalSettings.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = this.Width;
            graphics.PreferredBackBufferHeight = this.Height;
            Initialize();
        }

        public void SetFullScreenResolution()
        {
            fullScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            fullScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        private void SetToWindowed()
        {
            renderTargetRect = RenderTargetWindowRect();
            this.Width = GlobalSettings.WINDOW_WIDTH;
            this.Height = GlobalSettings.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = this.Width;
            graphics.PreferredBackBufferHeight = this.Height;
            graphics.ApplyChanges();
            window.IsBorderless = false;
            window.Position = new Point((fullScreenWidth - GlobalSettings.WINDOW_WIDTH) / 2,
                                        (fullScreenHeight - GlobalSettings.WINDOW_HEIGHT) / 2);

        }

        private Rectangle RenderTargetWindowRect()
        {
            return new Rectangle(0, 0, GlobalSettings.WINDOW_WIDTH, GlobalSettings.WINDOW_HEIGHT);
        }

        internal void Initialize()
        {
            SetFullScreenResolution();
            SetToWindowed();
        }

        public void ToggleFullScreen()
        {
            if (window.IsBorderless)
            {
                SetToWindowed();
            }
            else
            {
                SetToFullScreen();
            }
        }

        private void SetToFullScreen()
        {
            renderTargetRect = RenderTargetFullScreenRect();
            this.Width = fullScreenWidth;
            this.Height = fullScreenHeight;
            graphics.PreferredBackBufferWidth = this.Width;
            graphics.PreferredBackBufferHeight = this.Height;
            graphics.ApplyChanges();
            window.IsBorderless = true;
            window.Position = new Point(0, 0);

        }

        private Rectangle RenderTargetFullScreenRect()
        {
            if (fullScreenWidth > fullScreenHeight)
            {
                int newWindowWidth = (int)System.Math.Ceiling(
                    ((float)fullScreenHeight / (float)GlobalSettings.WINDOW_HEIGHT) * GlobalSettings.WINDOW_WIDTH);

                return new Rectangle((fullScreenWidth - newWindowWidth) / 2,
                    0, newWindowWidth, fullScreenHeight);
            }
            else
            {
                int newWindowHeight = (int)System.Math.Ceiling(
                    ((float)fullScreenWidth / (float)GlobalSettings.WINDOW_WIDTH) * GlobalSettings.WINDOW_HEIGHT);

                return new Rectangle(0, (fullScreenHeight - newWindowHeight) / 2,
                    fullScreenWidth, newWindowHeight);
            }
        }

    }
}
