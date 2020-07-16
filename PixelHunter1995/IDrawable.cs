using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.SceneLib;

namespace PixelHunter1995
{
    interface IDrawable
    {
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Tileset tileset);
    }
}
