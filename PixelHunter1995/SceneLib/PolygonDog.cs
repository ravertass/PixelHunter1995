using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.SceneLib
{
    class PolygonDog : IDog
    {
        private readonly int X;
        private readonly int Y;
        private List<Coord> Points;
        public PolygonDog(int x, int y, List<Coord> points)
        {
            X = x;
            Y = y;
            Points = points;
        }
    }
}
