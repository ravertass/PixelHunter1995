using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.WalkingAreaLib;

namespace PixelHunter1995.SceneLib
{
    public class Portal : IDrawable
    {
        private readonly int X;
        private readonly int Y;
        private readonly Polygon Polygon;
        private readonly int sceneWidth;
        public string Name { get; private set; }
        public string DestinationScene { get; private set; }
        public string DestinationPortal { get; private set; }
        // TODO: This should probably be set through Tiled properties.
        public Vector2 AppearancePosition { get => new Vector2(X, Y); }

        public Portal(int x,
                      int y,
                      List<Vector2> points,
                      int sceneWidth,
                      string name,
                      string destinationScene,
                      string destinationPortal)
        {
            X = x;
            Y = y;
            this.sceneWidth = sceneWidth;
            Polygon = new Polygon(points);
            Name = name;
            DestinationScene = destinationScene;
            DestinationPortal = destinationPortal;
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
            return 10;
        }
    }
}
