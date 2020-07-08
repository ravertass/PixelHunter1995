

namespace PixelHunter1995.Components.Gamma
{
    interface IHasComponentGamma<TComponent> : IHasComponentsGamma
        where TComponent : IComponentGamma
    {

        TComponent Component { get; set; }

    }
}
