using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Components;
using PixelHunter1995.SceneLib;
using System;

namespace PixelHunter1995
{

    class Player : IUpdateable, IDrawable, ILoadContent, IHasComponent<PositionComponent>, IHasComponent<SpriteComponent>, ISpriteComponent
    {
        Vector2 MovePosition { get; set; }

        private PositionComponent PosComp { get; set; }
        private SpriteComponent SpriteComp { get; set; }

        // alias
        private Vector2 Position { get => this.PosComp.Position; set => this.PosComp.Position = value; }

        PositionComponent IHasComponent<PositionComponent>.Component => PosComp;
        SpriteComponent IHasComponent<SpriteComponent>.Component => SpriteComp;

        public Texture2D Sprite
        {
            get => SpriteComp.Sprite;
            set => SpriteComp.Sprite = value;
        }

        private readonly Game game;

        public Player(Game game) : this(game, 0, 0) { }
        public Player(Game game, float x, float y)
        {
            this.PosComp = new PositionComponent();
            this.SpriteComp = new SpriteComponent(this.PosComp);

            this.game = game;

            this.Position = new Vector2(x, y);
            this.MovePosition = this.Position;
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (game.IsActive && mouseState.LeftButton == ButtonState.Pressed)
            {
                this.MovePosition = new Vector2(mouseState.X, mouseState.Y);
            }

            this.Position = this.Approach(Position, MovePosition, 2);

        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Tileset tileset)
        {
            SpriteComp.Draw(graphics, spriteBatch, tileset);
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
                //return start + new Vector2(dir.X * speed, dir.Y * speed);
                return start + dir * (float)speed;
            }
        }

        // TODO Figure out some way to handle this better
        public void LoadContent(ContentManager content)
        {
            this.Sprite = content.Load<Texture2D>("Images/snubbe");
        }
    }
}
