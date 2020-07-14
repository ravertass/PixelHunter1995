using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Net.Mime;

namespace PixelHunter1995
{
    using Dog = System.ValueTuple<float, float, float, float>;

    class Scene : IDrawable, IUpdateable, ILoadContent
    {
        private List<IDrawable> drawables;
        private List<IUpdateable> updateables;
        private List<ILoadContent> loadables;

        public Scene(List<IDrawable> drawables, List<IUpdateable> updateables, List<ILoadContent> loadables)
        {
            this.drawables = drawables;
            this.updateables = updateables;
            this.loadables = loadables;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            foreach (IDrawable drawable in drawables)
            {
                drawable.Draw(graphics, spriteBatch);
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
            foreach (ILoadContent loadable in loadables)
            {
                loadable.LoadContent(content); ;
            }
        }
    }
}
