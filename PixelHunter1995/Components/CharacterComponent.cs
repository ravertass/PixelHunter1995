using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;
using System;

namespace PixelHunter1995.Components
{
    class CharacterComponent : IComponent, ICharacterComponent
    {
        private PositionComponent PositionComponent { get; set; }
        public Vector2 MoveDirection { get; set; }
        public AnimationTileset AnimationTileset { get; set; }
        public Color FontColor { get; set; }
        public String FontName { get; set; }

        // alias
        private Vector2 Position { get => this.PositionComponent.Position; }

        public CharacterComponent(PositionComponent posComp)
        {
            this.PositionComponent = this.NotNullDependency(posComp, "posComp");
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            var animationTileset = this.AnimationTileset;

            if (animationTileset == null)
            {
                // TODO Create some error-texture, drawn when a texture is missing (like gmods checkerboard texture)
                Console.Error.WriteLine(String.Format("ERROR! - attempted to draw an animation with no tileset! CharachterComponent: {0}", this));
                return;
            }

            animationTileset.Draw(spriteBatch, Position, MoveDirection, scaling);
        }

        public void LoadContent(ContentManager content)
        {
            AnimationTileset.LoadContent(content);
        }

        private Vector2 RelativePosition(int deltaX, int deltaY)
        {
            return new Vector2(Position.X + deltaX, Position.Y + deltaY);
        }

        public void Talk(SpriteBatch spriteBatch, string text)
        {
            SpriteFont font = FontManager.Instance.getFontByName(FontName);
            int textDeltaX = -(int)font.MeasureString(text).X / 2 + AnimationTileset.tileWidth / 2;
            int textDeltaY = -(int)font.MeasureString(text).Y - 5;
            // Draw black around the letters to see them better
            spriteBatch.DrawString(font, text, RelativePosition(textDeltaX + 1, textDeltaY + 1), Color.Black);
            spriteBatch.DrawString(font, text, RelativePosition(textDeltaX - 1, textDeltaY - 1), Color.Black);
            spriteBatch.DrawString(font, text, RelativePosition(textDeltaX, textDeltaY), FontColor);
        }

        public int ZOrder()
        {
            return (int)Position.Y;
        }
    }
}
