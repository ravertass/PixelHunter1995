using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.SceneLib
{
    internal class HoverText : IDrawable
    {
        private bool Active = false;
        private string Text;
        private static readonly int X_POS = GlobalSettings.WINDOW_WIDTH/2;
        private static readonly int Y_POS = 160;

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            if (!Active)
            {
                return;
            }

            SpriteFont font = FontManager.Instance.getFontByName("FreePixel");
            int deltaX = -(int)font.MeasureString(Text).X / 2;
            spriteBatch.DrawString(font, Text, new Vector2(X_POS + deltaX, Y_POS), Color.Purple);
        }

        internal void Update(InputManager input, List<IDog> dogs)
        {
            Coord mousePos = new Coord(input.MouseX, input.MouseY);
            Active = false;
            foreach (IDog dog in dogs)
            {
                if (dog.Contains(mousePos))
                {
                    Active = true;
                    Text = dog.Name;
                    break;
                }
            }
        }

        public int ZIndex()
        {
            return 10;
        }
    }
}
