﻿using Microsoft.Xna.Framework;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.Components
{
    interface ICharacterComponent : IDrawable, ITalkative
    {

        AnimationTileset AnimationTileset { get; set; }
        Vector2 MoveDirection { get; set; }

    }
}
