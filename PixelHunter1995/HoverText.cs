using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Utilities;

namespace PixelHunter1995
{
    internal class HoverText : IDrawable
    {
        private bool Active = false;
        private string Text;
        private static readonly int X_POS = GlobalSettings.WINDOW_WIDTH/2;
        private static readonly int Y_POS = GlobalSettings.SCENE_HEIGHT;

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

        public void SetText(string text)
        {
            Active = true;
            Text = text;
        }

        public void UnSetText()
        {
            Active = false;
        }

        public int ZIndex()
        {
            return 10;
        }
    }
}
