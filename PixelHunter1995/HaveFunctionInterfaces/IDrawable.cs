using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995
{
    interface IDrawable
    {
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scalingMin);
        int ZIndex();
    }
}
