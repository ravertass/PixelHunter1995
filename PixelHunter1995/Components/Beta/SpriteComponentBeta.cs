using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelHunter1995.Components.Beta
{
    class SpriteComponentBeta : IComponentBeta, IDrawable
    {
        public Texture2D Sprite { get; set; }

        private PositionComponentBeta _positionComponent;

        private Vector2 Position { get => this._positionComponent.Position; }

        public SpriteComponentBeta(PositionComponentBeta posComp)
        {
            if (posComp == null)
            {
                throw new ArgumentNullException("SpriteComponentBeta was given a null value for a dependency");
            }

            this._positionComponent = posComp;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, this.Position, Color.White);
        }
    }
}
