

using Microsoft.Xna.Framework;
using System;

namespace PixelHunter1995.Components.Alpha
{
    class PositionComponentAlpha : IComponentAlpha
    {

        public Vector2 Position { get; set; }

        public PositionComponentAlpha()
        {

        }

        public PositionComponentAlpha SetOwner<T>(T owner)
            where T : IHasComponentsAlpha, IHasComponentAlpha<PositionComponentAlpha>
        {
            Console.WriteLine("blah2");
            return this;
        }
    }
}
