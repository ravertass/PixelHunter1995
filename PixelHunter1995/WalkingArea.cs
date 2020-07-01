using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class WalkingArea : IDrawable
    {
        private List<Polygon> convexPolygons;

        public WalkingArea(Polygon polygon)
        {
            this.convexPolygons = polygon.ConvexPartition();
        }

        public WalkingArea(List<Coord> points)
            : this(new Polygon(points))
        {
        }

        public override string ToString()
        {
            return String.Join<Polygon>("; ", convexPolygons);
        }

        public void Draw(GraphicsDeviceManager graphics)
        {
            convexPolygons.ForEach(p => p.Draw(graphics));
        }
    }
}
