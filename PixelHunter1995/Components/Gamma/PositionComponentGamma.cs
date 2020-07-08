

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PixelHunter1995.Components.Gamma
{
    class PositionComponentGamma : IComponentGamma
    {

        public Vector2 Position { get; set; }

        public HashSet<Type> Dependencies { get; }

        public PositionComponentGamma()
        {
            Console.WriteLine("PositionComponentGamma - Constructor");
        }

        public void Init(CompositeGamma owner)
        {
            Console.WriteLine("PositionComponentGamma - Init");
        }
    }
}
