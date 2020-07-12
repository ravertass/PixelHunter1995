using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;

namespace PixelHunter1995
{
    internal class Screen
    {
        internal int fullScreenWidth;
        internal int fullScreenHeight;
        public const int WINDOW_WIDTH = 426;
        public const int WINDOW_HEIGHT = 240;
        public Rectangle renderTargetRect;
        private readonly GraphicsDeviceManager graphics;
        private readonly GameWindow window;
        private bool justToggledFullscreen = false;

        public Screen(GraphicsDeviceManager graphics, GameWindow window)
        {
            this.graphics = graphics;
            this.window = window;
            graphics.PreferredBackBufferWidth = Screen.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Screen.WINDOW_HEIGHT;
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
            graphics.PreferredBackBufferWidth = Screen.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Screen.WINDOW_HEIGHT;
            graphics.ApplyChanges();
            window.IsBorderless = false;
            window.Position = new Point((fullScreenWidth - Screen.WINDOW_WIDTH) / 2,
                                        (fullScreenHeight - Screen.WINDOW_HEIGHT) / 2);

        }

        private Rectangle RenderTargetWindowRect()
        {
            return new Rectangle(0, 0, Screen.WINDOW_WIDTH, Screen.WINDOW_HEIGHT);
        }

        internal void Initialize()
        {
            SetFullScreenResolution();
            SetToWindowed();
        }

        public void ToggleFullScreen()
        {
            if (!window.IsBorderless)
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
            window.IsBorderless = true;
            window.Position = new Point(0, 0);

        }

        private Rectangle RenderTargetFullScreenRect()
        {
            if (fullScreenWidth > fullScreenHeight)
            {
                int newWindowWidth = (int)System.Math.Ceiling(
                    ((float)fullScreenHeight / (float)Screen.WINDOW_HEIGHT) * Screen.WINDOW_WIDTH);

                return new Rectangle((fullScreenWidth - newWindowWidth) / 2,
                    0, newWindowWidth, fullScreenHeight);
            }
            else
            {
                int newWindowHeight = (int)System.Math.Ceiling(
                    ((float)fullScreenWidth / (float)Screen.WINDOW_WIDTH) * Screen.WINDOW_HEIGHT);

                return new Rectangle(0, (fullScreenHeight - newWindowHeight) / 2,
                    fullScreenWidth, newWindowHeight);
            }
        }

        private bool IsAltEnterPressed(KeyboardState state)
        {
            return (state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt)) && state.IsKeyDown(Keys.Enter);
        }

        public void CheckForFullScreen()
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

    }
}