

namespace PixelHunter1995.Components
{
    /// <summary>
    /// An interface that ensures an IComponent can be accessed from a property.
    /// Not required to implement when using components, and is unable to expose several components of same type.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    interface IHasComponent<TComponent> : IHasComponents
        where TComponent : IComponent
    {

        /// <summary>
        /// A Property that exposes the component.
        /// </summary>
        TComponent Component { get; }

    }
}
