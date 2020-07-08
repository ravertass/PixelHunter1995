

using Microsoft.Xna.Framework;
using System;

namespace PixelHunter1995.Components
{
    class PositionComponent : IComponent
    {

        public Vector2 Position { get; set; }

        public PositionComponent()
        {

        }

        public PositionComponent SetOwner<T>(T owner)
            where T : IHasComponents, IHasComponent<PositionComponent>
        {
            Console.WriteLine("blah2");
            return this;
        }
    }
}
