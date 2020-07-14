
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Background : IDrawable, ILoadContent
    {
        string imagePath;
        int width;
        int height;
        public Texture2D image;
        public Background(string imagePath, int width, int height)
        {
            this.imagePath = imagePath;
            this.width = width;
            this.height = height;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>(imagePath);
        }
    }
}
