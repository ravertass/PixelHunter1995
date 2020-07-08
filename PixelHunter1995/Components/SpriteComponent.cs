using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelHunter1995.Components
{
    class SpriteComponent : IComponent, IDrawable
    {
        public Texture2D Sprite { get; set; }

        private PositionComponent _positionComponent;

        private Vector2 Position { get => this._positionComponent.Position; }

        public SpriteComponent()
        {
            //this.SetOwner(owner);
        }

        //! Dependency-method can't be imposed by an interface,
        //! as the generic type has to be exactly the same :(
        //! There is also no way to ensure it is called, unlike constructors.
        //public IComponentGamma SetOwner<T>(T owner)
        public SpriteComponent SetOwner<T>(T owner)
            where T : IHasComponents, IHasComponent<SpriteComponent>, IHasComponent<PositionComponent>
        {
            this._positionComponent = ((IHasComponent<PositionComponent>) owner).Component;
            //this._positionComponent = owner.GetComponent<PositionComponent>();
            Console.WriteLine("blah1");
            return this;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Sprite, this.Position, Color.White);

        }
    }
}
