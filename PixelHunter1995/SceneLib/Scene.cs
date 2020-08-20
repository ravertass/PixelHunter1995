using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.SceneLib;
using PixelHunter1995.Utilities;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene : IUpdateable, ILoadContent
    {
        private List<IDrawable> Drawables;
        private List<IUpdateable> Updateables;
        private List<ILoadContent> Loadables;
        private List<IDog> Dogs;
        private HoverText HoverText;

        public Scene(List<IDrawable> drawables,
                     List<IUpdateable> updateables,
                     List<ILoadContent> loadables,
                     List<IDog> dogs)
        {
            Drawables = drawables;
            Updateables = updateables;
            Loadables = loadables;
            Dogs = dogs;
            HoverText = new HoverText();
            Drawables.Add(HoverText);
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

            Coord mousePos = new Coord(input.MouseX, input.MouseY);
            bool foundADog = false;
            foreach (IDog dog in Dogs)
            {
                if (dog.Contains(mousePos))
                {
                    HoverText.Activate("Apa");
                    foundADog = true;
                    break;
                }
            }
            if (!foundADog)
            {
                HoverText.Deactivate();
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (ILoadContent loadable in this.Loadables)
            {
                loadable.LoadContent(content); ;
            }
        }
    }
}
