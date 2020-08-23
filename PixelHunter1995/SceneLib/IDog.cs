using PixelHunter1995.Utilities;

namespace PixelHunter1995.SceneLib
{
    internal interface IDog : IDrawable
    {
        bool Contains(Coord point);
        string Name { get; }
    }
}
