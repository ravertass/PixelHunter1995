using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.Components
{
    interface ICharacterComponent : IDrawable, ITalkable
    {

        AnimationTileset AnimationTileset { get; set; }
        Vector2 MoveDirection { get; set; }

    }
}
