using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Utilities;
using PixelHunter1995.WalkingAreaLib;

namespace PixelHunter1995.SceneLib
{
    class PolygonDog : IDog
    {
        private readonly int X;
        private readonly int Y;
        private readonly Polygon Polygon;
        private readonly int sceneWidth;

        public PolygonDog(int x, int y, List<Coord> points, int sceneWidth)
        {
            X = x;
            Y = y;
            this.sceneWidth = sceneWidth;
            Polygon = new Polygon(points);
            Name = "PolygonDog";
        }

        public string Name { get; }

        public bool Contains(Coord point)
        {
            return Polygon.Contains(point);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            Polygon.Draw(graphics, spriteBatch, scaling, sceneWidth);
        }

        public int ZIndex()
        {
            return Y;
        }
    }
}
