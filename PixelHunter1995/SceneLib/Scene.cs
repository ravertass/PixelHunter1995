using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.SceneLib;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene : ILoadContent
    {
        private List<IDrawable> Drawables;
        private List<IUpdateable> Updateables;
        private List<ILoadContent> Loadables;
        public List<IDog> Dogs;
        public Player Player;
        public int Width { get; private set; }

        public Scene(List<IDrawable> drawables,
                     List<IUpdateable> updateables,
                     List<ILoadContent> loadables,
                     List<IDog> dogs,
                     Player player,
                     int width)
        {
            Player = player;
            Drawables = drawables;
            Drawables.Add(player);
            Updateables = updateables;
            Loadables = loadables;
            Loadables.Add(player);
            Dogs = dogs;
            Player = player;
            Width = width;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            // We sort on Z and draw lowest first.
            Drawables.Sort((a, b) => a.ZIndex().CompareTo(b.ZIndex()));
            foreach (IDrawable drawable in this.Drawables)
            {
                drawable.Draw(graphics, spriteBatch, scaling);
            }
        }

        public void Update(GameTime gameTime, InputManager input, bool playerControllable)
        {
            Player.Update(gameTime, input, playerControllable);
            foreach (IUpdateable updateable in this.Updateables)
            {
                updateable.Update(gameTime, input);
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (ILoadContent loadable in this.Loadables)
            {
                loadable.LoadContent(content);
            }
        }
    }
}
