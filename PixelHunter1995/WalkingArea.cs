using System;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class WalkingArea
    {
        private List<Coord> points;

        public WalkingArea(List<Coord> points)
        {
            this.points = points;
        }

        public override string ToString()
        {
            return String.Join<Coord>(", ", points);
        }
    }
}
