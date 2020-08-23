using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.SceneLib;
using PixelHunter1995.Utilities;

namespace PixelHunter1995
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

        internal void Update(InputManager input, List<IDog> dogs, List<InventoryItem> items)
        {
            Coord mousePos = new Coord(input.MouseX, input.MouseY);
            Active = false;
            // We sort on Z index, to check the dog on top first. Note that this is reversed from
            // when we draw them, since in that case we want to draw the thing on top last.
            dogs.Sort((a, b) => b.ZIndex().CompareTo(a.ZIndex()));
            foreach (IDog dog in dogs.Concat(items)) // We need not sort the inventory items, since they have their own space
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
