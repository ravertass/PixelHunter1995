using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.SceneLib
{
    class Dog : IDrawable
    {
        private readonly int X;
        private readonly int Y;
        private readonly int Width;
        private readonly int Height;
        private readonly int Gid;
        private readonly Tileset Tileset;

        public Dog(int x, int y, int width, int height, int gid, Tileset tileset)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Gid = gid;
            this.Tileset = tileset;
        }
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            Tileset.Draw(spriteBatch, new Vector2(X, Y), Gid, scaling);
        }
    }
}
