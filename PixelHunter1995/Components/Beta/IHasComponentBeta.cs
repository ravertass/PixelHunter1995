

namespace PixelHunter1995.Components.Beta
{
    /// <summary>
    /// An interface that ensures an IComponent can be accessed from a property.
    /// Not required to implement when using components, and is unable to expose several components of same type.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    interface IHasComponentBeta<TComponent> : IHasComponentsBeta
        where TComponent : IComponentBeta
    {

        /// <summary>
        /// A Property that exposes the component.
        /// </summary>
        TComponent Component { get; }

    }
}
