using Microsoft.Xna.Framework;

namespace PixelHunter1995.SceneLib
{
    internal interface IDog : IDrawable
    {
        bool Contains(Vector2 point);
        string Name { get; }
    }
}
