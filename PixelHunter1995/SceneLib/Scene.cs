using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.SceneLib;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene
    {
        public Background background;
        public List<Dog> dogs;
        public WalkingArea walkingArea;
        public Tileset tileset;

        public Scene(Background background, List<Dog> dogs, WalkingArea walkingArea, Tileset tileset)
        {
            this.background = background;
            this.dogs = dogs;
            this.walkingArea = walkingArea;
            this.tileset = tileset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            foreach (Dog dog in dogs)
            {
                dog.Draw(spriteBatch, tileset);
            }
        }

        public void LoadContent(ContentManager content)
        {
            background.LoadContent(content);
            tileset.LoadContent(content);
        }
    }
}
