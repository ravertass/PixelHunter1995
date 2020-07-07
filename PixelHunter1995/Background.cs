
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Background
    {
        public string ImagePath { get; private set; }
        int width;
        int height;
        public Texture2D image;
        public Background(string image, int width, int height)
        {
            this.ImagePath = image;
            this.width = width;
            this.height = height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>(ImagePath);
        }
    }
}
