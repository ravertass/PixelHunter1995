using Microsoft.Xna.Framework;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.Inputs
{
    class FullscreenFix
    {

        public static Screen Screen { private get; set; } //! ugly, I know...

        public static int GetFixedX(int x)
        {
            double ratioWidth = GlobalSettings.WINDOW_WIDTH / (double)Screen.Width;
            return (int)(x * ratioWidth);
        }
        public static int GetFixedY(int y)
        {
            double ratioHeight = GlobalSettings.WINDOW_HEIGHT / (double)Screen.Height;
            return (int)(y * ratioHeight);
        }
        public static Point GetFixedPoint(Point p)
        {
            return new Point(GetFixedX(p.X), GetFixedY(p.Y));
        }
    }
}
