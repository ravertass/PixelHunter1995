using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.SceneLib;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene : IUpdateable, ILoadContent
    {
        private List<IDrawable> drawables;
        private List<IUpdateable> updateables;
        private List<ILoadContent> loadables;
        public Tileset tileset;

        public Scene(List<IDrawable> drawables, List<IUpdateable> updateables, List<ILoadContent> loadables, Tileset tileset)
        {
            this.drawables = drawables;
            this.updateables = updateables;
            this.loadables = loadables;
            this.tileset = tileset;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            foreach (IDrawable drawable in drawables)
            {
                drawable.Draw(graphics, spriteBatch, tileset);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IUpdateable updateable in updateables)
            {
                updateable.Update(gameTime);
            }
        }

        public void LoadContent(ContentManager content)
        {
            tileset.LoadContent(content);
            foreach (ILoadContent loadable in loadables)
            {
                loadable.LoadContent(content); ;
            }
        }
    }
}
