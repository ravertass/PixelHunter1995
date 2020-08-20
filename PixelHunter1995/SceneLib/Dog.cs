using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.SceneLib
{
    class Dog : IDog, IDrawable
    {
        private readonly int X;
        private readonly int Y;
        private readonly int Width;
        private readonly int Height;
        private readonly int Gid;
        private readonly Tileset Tileset;
        public string Name { get; }

        public Dog(int x, int y, int width, int height, int gid, Tileset tileset)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Gid = gid;
            Tileset = tileset;
            Name = "Dog " + Gid;
        }


        public bool Contains(Coord point)
        {
            return new Rectangle(X, Y, Width, Height).Contains(new Point((int)point.X, (int)point.Y));
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            Tileset.Draw(spriteBatch, new Vector2(X, Y), Gid, scaling);
        }

        public int ZIndex()
        {
            return Y + Height;
        }
    }
}
