using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.WalkingAreaLib;

namespace PixelHunter1995.SceneLib
{
    class PolygonDog : IDog
    {
        private readonly int X;
        private readonly int Y;
        private readonly Polygon Polygon;
        private readonly int sceneWidth;
        public string Name { get; }

        public PolygonDog(int x, int y, List<Vector2> points, int sceneWidth, string name)
        {
            X = x;
            Y = y;
            this.sceneWidth = sceneWidth;
            Polygon = new Polygon(points);
            Name = name;
        }

        public bool Contains(Vector2 point)
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
