

using System;
using System.Collections.Generic;

namespace PixelHunter1995.Components.Gamma
{
    interface IComponentGamma
    {
        HashSet<Type> Dependencies { get; }

        void Init(CompositeGamma owner);
    }
}
