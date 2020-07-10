using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelHunter1995.Components.Beta
{
    class SpriteComponentBeta : IComponentBeta, IDrawable
    {
        public Texture2D Sprite { get; set; }

        public PositionComponentBeta PositionComponent { get; set; }

        // alias
        private Vector2 Position { get => this.PositionComponent.Position; }

        public SpriteComponentBeta(PositionComponentBeta posComp)
        {
            this.PositionComponent = this.NotNullDependency(posComp, "posComp");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, this.Position, Color.White);
        }
    }
}
