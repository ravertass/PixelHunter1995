using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene : IInputHandler, IUpdateable, ILoadContent
    {
        private List<IDrawable> drawables;
        private List<IInputHandler> inputhandlers;
        private List<IUpdateable> updateables;
        private List<ILoadContent> loadables;

        public Scene(List<IDrawable> drawables, List<IInputHandler> inputhandlers, List<IUpdateable> updateables, List<ILoadContent> loadables)
        {
            this.drawables = drawables;
            this.inputhandlers = inputhandlers;
            this.updateables = updateables;
            this.loadables = loadables;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            // We sort on Z and draw lowest first.
            drawables.Sort((a, b) => a.ZIndex().CompareTo(b.ZIndex()));
            foreach (IDrawable drawable in this.drawables)
            {
                drawable.Draw(graphics, spriteBatch, scaling);
            }
        }

        public void HandleInput(GameTime gameTime, Input input)
        {
            foreach (IInputHandler inputhandler in this.inputhandlers)
            {
                inputhandler.HandleInput(gameTime, input);
            }
        }

        public void Update(GameTime gameTime, Input input)
        {
            foreach (IUpdateable updateable in this.updateables)
            {
                updateable.Update(gameTime, input);
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (ILoadContent loadable in this.loadables)
            {
                loadable.LoadContent(content); ;
            }
        }
    }
}
