

namespace PixelHunter1995.Components.Alpha
{
    interface IHasComponentAlpha<TComponent> : IHasComponentsAlpha
        where TComponent : IComponentAlpha
    {

        TComponent Component { get; set; }

    }
}
