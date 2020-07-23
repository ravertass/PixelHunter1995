using Microsoft.Xna.Framework;
using PixelHunter1995.Inputs;

namespace PixelHunter1995
{
    interface IUpdateable
    {
        void Update(GameTime gameTime, Input input);
    }
}
