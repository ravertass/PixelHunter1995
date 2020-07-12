using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelHunter1995.Components
{
    class SpriteComponent : IComponent, IDrawable
    {
        public Texture2D Sprite { get; set; }

        public PositionComponent PositionComponent { get; set; }

        // alias
        private Vector2 Position { get => this.PositionComponent.Position; }

        public SpriteComponent(PositionComponent posComp)
        {
            this.PositionComponent = this.NotNullDependency(posComp, "posComp");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var sprite = this.Sprite;

            if (sprite == null)
            {
                // TODO Create some error-texture, drawn when a texture is missing (like gmods checkerboard texture)
                Console.Error.WriteLine(String.Format("ERROR! - attempted to draw a sprite with no texture! SpriteComponent: {1}", this));
                return;
            }

            spriteBatch.Draw(sprite, this.Position, Color.White);
        }
    }
}
