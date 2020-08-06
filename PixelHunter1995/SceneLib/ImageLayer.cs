
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995.SceneLib
{
    class ImageLayer : IDrawable, ILoadContent
    {
        string imagePath;
        int width;
        int height;
        int z;
        public Texture2D image;
        public ImageLayer(string imagePath, int width, int height, int z)
        {
            this.imagePath = imagePath;
            this.width = width;
            this.height = height;
            this.z = z;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>(imagePath);
        }

        public int ZIndex()
        {
            return z;
        }
    }
}
