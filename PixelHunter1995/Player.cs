using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Components;
using PixelHunter1995.TilesetLib;
using System;

namespace PixelHunter1995
{

    class Player : IUpdateable, IDrawable, ILoadContent, IHasComponent<PositionComponent>, IHasComponent<CharacterComponent>, ICharacterComponent
    {

        private Vector2 MovePosition { get; set; }

        private PositionComponent PosComp { get; set; }
        private CharacterComponent CharComp { get; set; }

        // alias
        private Vector2 Position { get => this.PosComp.Position; set => this.PosComp.Position = value; }
        public Vector2 MoveDirection { get => this.CharComp.MoveDirection; set => this.CharComp.MoveDirection = value; }
        public Color FontColor { get => this.CharComp.FontColor; set => this.CharComp.FontColor = value; }
        public String FontName { get => this.CharComp.FontName; set => this.CharComp.FontName = value; }
        public AnimationTileset AnimationTileset
        {
            get => this.CharComp.AnimationTileset;
            set => this.CharComp.AnimationTileset = value;
        }

        PositionComponent IHasComponent<PositionComponent>.Component => PosComp;
        CharacterComponent IHasComponent<CharacterComponent>.Component => CharComp;

        private readonly Game game;

        public Player(Game game) : this(game, 0, 0) { }
        public Player(Game game, float x, float y)
        {
            this.PosComp = new PositionComponent();
            this.CharComp = new CharacterComponent(this.PosComp);

            this.game = game;

            this.Position = new Vector2(x, y);
            this.MovePosition = this.Position;
            this.MoveDirection = new Vector2();

            this.FontColor = Color.Purple;
            this.FontName = "Alkhemikal";
            this.AnimationTileset = new AnimationTileset("Animations/felixia");
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (game.IsActive && mouseState.LeftButton == ButtonState.Pressed)
            {
                this.MovePosition = new Vector2(mouseState.X, mouseState.Y);
            }
            this.MoveDirection = MovePosition - Position;
            this.Position = this.Approach(Position, MovePosition, 2);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            CharComp.Draw(graphics, spriteBatch, scaling);
            Talk(spriteBatch, "Hi, I'm the player!"); // TODO: Get text from some kind of file
        }

        public void Talk(SpriteBatch spriteBatch, String text)
        {
            CharComp.Talk(spriteBatch, text);
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
