using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Components;
using PixelHunter1995.TilesetLib;
using PixelHunter1995.Inputs;
using System;

namespace PixelHunter1995
{

    class Player : IDrawable, ILoadContent, IHasComponent<PositionComponent>, IHasComponent<CharacterComponent>, ICharacterComponent
    {
        private Vector2 MovePosition { get; set; }

        private PositionComponent PosComp { get; set; }
        private CharacterComponent CharComp { get; set; }

        // alias
        public Vector2 Position { get => this.PosComp.Position; private set => this.PosComp.Position = value; }
        public Vector2 MoveDirection { get => this.CharComp.MoveDirection; set => this.CharComp.MoveDirection = value; }
        public Color FontColor { get => this.CharComp.FontColor; set => this.CharComp.FontColor = value; }
        public String FontName { get => this.CharComp.FontName; set => this.CharComp.FontName = value; }
        public Vector2 FeetPosition { get => CharComp.FeetPosition; }

        public AnimationTileset AnimationTileset
        {
            get => this.CharComp.AnimationTileset;
            set => this.CharComp.AnimationTileset = value;
        }

        PositionComponent IHasComponent<PositionComponent>.Component => PosComp;
        CharacterComponent IHasComponent<CharacterComponent>.Component => CharComp;

        public Player() : this(0, 0) { }
        public Player(float x, float y)
        {
            this.PosComp = new PositionComponent();
            this.CharComp = new CharacterComponent(this.PosComp);

            this.Position = new Vector2(x, y);
            this.MovePosition = this.Position;
            this.MoveDirection = new Vector2();

            this.FontColor = Color.Purple;
            this.FontName = "Alkhemikal";
            this.AnimationTileset = new AnimationTileset("Animations/felixia");
        }

        public void Update(GameTime gameTime, InputManager input, bool controllable)
        {
            CharComp.Update(gameTime, input);
            if (controllable)
            {
                HandleInput(input);
            }
            this.MoveDirection = MovePosition - Position;
            this.Position = this.Approach(Position, MovePosition, 2);
        }

        public void HandleInput(InputManager input)
        {
            if (input.GetState(InputCommand.PLAYING_Move).IsDown)
            {
                // Compensate for Position being in top left corner
                float x = input.MouseX - AnimationTileset.tileWidth / 2;
                float y = input.MouseY - AnimationTileset.tileHeight;
                this.MovePosition = new Vector2(x, y);
            }
            if (input.Input.GetKeyState(MouseKeys.LeftButton).IsEdgeDown)
            {
                Say("Hi, I'm the player!");  // For debugging purposes, have the character talk when walking
                Say("Well, actually my name is Felixia.");
                Say("My interests include alchemy, solving ridiculously convoluted puzzles " +
                    "and long pull requests on the shore.");
            }
        }

        public void Say(string speech)
        {
            CharComp.Say(speech);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            CharComp.Draw(graphics, spriteBatch, scaling);
        }

        public Vector2 Approach(Vector2 start, Vector2 target, double speed)
        {
            Vector2 error = target - start;
            if (error.LengthSquared() <= Math.Pow(speed, 2))
            {
                return target;
            }
            else
            {
                Vector2 dir = Vector2.Normalize(error);
                return start + dir * (float)speed;
            }
        }

        public void LoadContent(ContentManager content)
        {
            CharComp.LoadContent(content);
        }

        public int ZIndex()
        {
            return CharComp.ZIndex();
        }
    }
}
