using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995
{
    class FontManager
    {
        private Dictionary<string, SpriteFont> Fonts;
        private static FontManager instance;

        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontManager();
                }
                return instance;
            }
        }

        private FontManager() {}

        public SpriteFont getFontByName(string fontName)
        {
            return Fonts[fontName];
        }

        public void LoadContent(ContentManager content)
        {
            Fonts = new Dictionary<string, SpriteFont>();
            // Add new fonts here
            var fontNames = new List<string> { "Alkhemikal", "FreePixel" };
            foreach (string fontName in fontNames)
            {
                var font = content.Load<SpriteFont>("Fonts/" + fontName);
                Fonts.Add(fontName, font);
            }
        }
    }
}
