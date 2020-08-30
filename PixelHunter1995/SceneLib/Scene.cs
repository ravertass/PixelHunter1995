﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.SceneLib;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene : IUpdateable, ILoadContent
    {
        private List<IDrawable> Drawables;
        private List<IUpdateable> Updateables;
        private List<ILoadContent> Loadables;
        public List<IDog> Dogs;
        // TODO: Should this be public? Would it even matter if it was gettable but not settable?
        public Player Player;
        public int Width { get; private set; }

        public Scene(List<IDrawable> drawables,
                     List<IUpdateable> updateables,
                     List<ILoadContent> loadables,
                     List<IDog> dogs,
                     Player player,
                     int width)
        {
            Drawables = drawables;
            Updateables = updateables;
            Loadables = loadables;
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

        public void Update(GameTime gameTime, InputManager input)
        {
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
