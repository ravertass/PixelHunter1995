using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class WalkingArea : IDrawable
    {
        private PolygonPartition partition;

        public WalkingArea(Polygon polygon)
        {
            partition = polygon.ConvexPartition();
        }

        public WalkingArea(List<Coord> points)
            : this(new Polygon(points))
        {
        }

        public override string ToString()
        {
            return partition.ToString();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            partition.Draw(graphics);
        }
    }
}
