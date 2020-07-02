using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PixelHunter1995
{
    using Dog = System.ValueTuple<float, float, float, float>;

    //using Background = System.ValueTuple<>;
    class Scene
    {
        public Background background;
        public List<Dog> dogs;
        public WalkingArea walkingArea;

        public Scene(Background background, List<Dog> dogs, WalkingArea walkingArea)
        {
            this.background = background;
            this.dogs = dogs;
            this.walkingArea = walkingArea;
        }

        public List<string> Images()
        {
            return new List<string>() { background.Image };
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
