using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelHunter1995.Components.Alpha
{
    class SpriteComponentAlpha : IComponentAlpha, IDrawable
    {
        public Texture2D Sprite { get; set; }

        private PositionComponentAlpha _positionComponent;

        private Vector2 Position { get => this._positionComponent.Position; }

        public SpriteComponentAlpha()
        {
            //this.SetOwner(owner);
        }

        //! Dependency-method can't be imposed by an interface,
        //! as the generic type has to be exactly the same :(
        //! There is also no way to ensure it is called, unlike constructors.
        //!  Can be alleviated with a builder/factory.
        //public IComponent SetOwner<T>(T owner)
        public SpriteComponentAlpha SetOwner<T>(T owner)
            where T : IHasComponentsAlpha, IHasComponentAlpha<SpriteComponentAlpha>, IHasComponentAlpha<PositionComponentAlpha>
        {
            this._positionComponent = ((IHasComponentAlpha<PositionComponentAlpha>) owner).Component;
            //this._positionComponent = owner.GetComponent<PositionComponentAlpha>();
            Console.WriteLine("blah1");
            return this;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Sprite, this.Position, Color.White);

        }
    }
}
