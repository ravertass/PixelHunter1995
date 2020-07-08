

namespace PixelHunter1995.Components.Beta
{
    interface IHasComponentBeta<TComponent> : IHasComponentsBeta
        where TComponent : IComponentBeta
    {

        /// <summary>
        /// A Property that allows accessing the component.
        /// </summary>
        TComponent Component { get; }

    }
}
