using System;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995.Components
{
    interface ITalkable
    {
        void Talk(SpriteBatch spriteBatch, String text);
    }
}
