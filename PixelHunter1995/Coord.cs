using System;

namespace PixelHunter1995
{
    public class Coord
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public Coord(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
