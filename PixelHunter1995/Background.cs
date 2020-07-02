
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Background
    {
        public string Image { get; private set; }
        int width;
        int height;
        public Background(string image, int width, int height)
        {
            this.Image = image;
            this.width = width;
            this.height = height;
        }

        public List<string> Content()
        {
            return new List<string>() { Image };
        }
    }
}
