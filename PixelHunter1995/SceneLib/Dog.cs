using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.SceneLib
{
    class Dog : IDog
    {
        private readonly int X;
        private readonly int Y;
        private readonly int Width;
        private readonly int Height;
        private readonly int Gid;
        private readonly Tileset Tileset;
        public string Name { get; }

        public Dog(int x, int y, int width, int height, int gid, Tileset tileset, string name)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Gid = gid;
            Tileset = tileset;
            Name = name;
        }

        public bool Contains(Vector2 point)
        {
            return new Rectangle(X, Y, Width, Height).Contains(point);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scalingMin)
        {
            Tileset.Draw(spriteBatch, new Vector2(X, Y), Gid, 1.0);
        }

        public int ZIndex()
        {
            return Y + Height;
        }
    }
}
