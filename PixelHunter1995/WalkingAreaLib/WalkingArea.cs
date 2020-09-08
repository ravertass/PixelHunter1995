using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PixelHunter1995.WalkingAreaLib
{
    class WalkingArea : IDrawable
    {
        private PolygonPartition partition;

        private int sceneWidth;

        public WalkingArea(Polygon polygon, int sceneWidth)
        {
            partition = polygon.ConvexPartition();
            this.sceneWidth = sceneWidth;
        }

        public WalkingArea(List<Vector2> points, int sceneWidth)
            : this(new Polygon(points), sceneWidth)
        {
        }

        public override string ToString()
        {
            return partition.ToString();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            partition.Draw(graphics, spriteBatch, scaling, sceneWidth);
        }

        public Vector2 GetNextPosition(Vector2 clickPosition, Vector2 currentPosition)
        {
            return partition.GetNextPosition(clickPosition, currentPosition);
        }

        public int ZIndex()
        {
            return 10;
        }
    }
}
