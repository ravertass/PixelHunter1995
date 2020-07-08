using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PixelHunter1995.Components.Gamma
{
    class SpriteComponentGamma : IComponentGamma, IDrawable
    {

        public Texture2D Sprite { get; set; }

        private PositionComponentGamma _positionComponent;

        private Vector2 Position { get => this._positionComponent.Position; }
        
        public HashSet<Type> Dependencies { get; }

        public SpriteComponentGamma()
        {
            Dependencies = new HashSet<Type>();

            Dependencies.Add(typeof(PositionComponentGamma));

            Console.WriteLine("SpriteComponentGamma - Constructor");
        }

        public void Init(CompositeGamma owner)
        {
            Console.WriteLine("SpriteComponentGamma - Init");

            /*
            if (!owner.HasComponent(typeof(PositionComponentGamma)))
            {
                throw new Exception("SpriteComponentGamma - Lacking dependencies!");
            }
            //*/

            this._positionComponent = owner.GetComponent<PositionComponentGamma>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Sprite, this.Position, Color.White);

        }
    }
}
