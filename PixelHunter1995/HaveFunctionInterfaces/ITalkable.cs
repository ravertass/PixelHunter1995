using System;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995
{
    interface ITalkable
    {
        void Talk(SpriteBatch spriteBatch, string text);
    }
}
