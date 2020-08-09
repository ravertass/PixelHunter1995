﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;
using System;

namespace PixelHunter1995.Components
{
    class CharacterComponent : IUpdateable, IComponent, ICharacterComponent
    {
        private PositionComponent PositionComponent { get; set; }
        public Vector2 MoveDirection { get; set; }
        public AnimationTileset AnimationTileset { get; set; }
        public Color FontColor { get; set; }
        public string FontName { get; set; }
        private readonly Voice Voice;

        // alias
        private Vector2 Position { get => this.PositionComponent.Position; }

        public CharacterComponent(PositionComponent posComp)
        {
            PositionComponent = this.NotNullDependency(posComp, "posComp");
            Voice = new Voice();
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
            DrawSpeech(spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            AnimationTileset.LoadContent(content);
        }

        public void Say(string speech)
        {
            Voice.Say(speech);
        }

        public void Update(GameTime gameTime)
        {
            Voice.Update(gameTime);
        }

        public void DrawSpeech(SpriteBatch spriteBatch)
        {
            int deltaX = AnimationTileset.tileWidth / 2;
            Vector2 charCenterPos = Position + new Vector2(deltaX, 0);
            Voice.Draw(spriteBatch, FontName, FontColor, charCenterPos);
        }

        public int ZIndex()
        {
            return (int)Position.Y + AnimationTileset.tileHeight;
        }
    }
}
