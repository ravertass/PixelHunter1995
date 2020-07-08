

namespace PixelHunter1995.Components
{
    interface IHasComponent<TComponent> : IHasComponents
        where TComponent : IComponent
    {

        TComponent Component { get; set; }

    }
}
