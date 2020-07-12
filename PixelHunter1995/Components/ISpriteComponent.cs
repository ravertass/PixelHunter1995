using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelHunter1995.Components
{
    interface ISpriteComponent : IDrawable
    {

        Texture2D Sprite { get; set; }

        PositionComponent PositionComponent { get; set; }

    }
}
