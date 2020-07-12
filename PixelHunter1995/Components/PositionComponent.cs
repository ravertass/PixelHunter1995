

using Microsoft.Xna.Framework;

namespace PixelHunter1995.Components
{
    class PositionComponent : IComponent
    {

        // TODO consider make the getter unable to return a null-value
        public Vector2 Position { get; set; }

        public PositionComponent()
        {

        }
    }
}
