using System;
using System.Globalization;

namespace PixelHunter1995
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // This makes floats with dots, not commas, the standard when parsing and writing strings.
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            using (var game = new Main())
                game.Run();
        }
    }
#endif
}
