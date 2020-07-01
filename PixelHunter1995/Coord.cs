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

        public override bool Equals(object other)
        {
            Coord otherCoord = other as Coord;

            if (otherCoord == null)
            {
                return false;
            }

            bool xEqual = ((X - otherCoord.X) < float.Epsilon) && ((otherCoord.X - X) < float.Epsilon);
            bool yEqual = ((Y - otherCoord.Y) < float.Epsilon) && ((otherCoord.Y - Y) < float.Epsilon);
            return xEqual && yEqual;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
