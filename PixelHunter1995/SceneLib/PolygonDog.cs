using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PixelHunter1995.Utilities;
using PixelHunter1995.WalkingAreaLib;

namespace PixelHunter1995.SceneLib
{
    class PolygonDog : IDog
    {
        private readonly int X;
        private readonly int Y;
        private Polygon Polygon;
        public PolygonDog(int x, int y, List<Coord> points)
        {
            X = x;
            Y = y;
            Polygon = new Polygon(points);
        }

        public bool Contains(Coord point)
        {
            return Polygon.Contains(point);
        }
    }
}
