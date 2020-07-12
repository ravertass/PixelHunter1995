using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelHunter1995
{
    /// <summary>
    /// Holder for global settings, e.g. if we're in debug mode.
    /// Use sparingly.
    /// Implements the singleton pattern.
    /// </summary>
    class GlobalSettings
    {
        private static GlobalSettings instance;
        public const int WINDOW_WIDTH = 426;
        public const int WINDOW_HEIGHT = 240;

        private GlobalSettings()
        {
        }

        public static GlobalSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalSettings();
                }

                return instance;
            }
        }

        public bool Debug { get; set; } = false;
    }
}
