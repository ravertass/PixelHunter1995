using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Components;
using System;

namespace PixelHunter1995
{

    class Player : IUpdateable, IDrawable, IHasComponent<PositionComponent>, IHasComponent<SpriteComponent>
    {
        Vector2 MovePosition { get; set; }

        private PositionComponent PosComp { get; set; }
        private SpriteComponent SpriteComp { get; set; }

        // alias
        private Vector2 Position { get => this.PosComp.Position; set => this.PosComp.Position = value; }

        PositionComponent IHasComponent<PositionComponent>.Component => PosComp;
        SpriteComponent IHasComponent<SpriteComponent>.Component => SpriteComp;

        private readonly Game game;

        public Player(Game game, Texture2D sprite) : this(game, sprite, 0, 0) { }
        public Player(Game game, Texture2D sprite, float x, float y)
        {
            this.PosComp = new PositionComponent();
            this.SpriteComp = new SpriteComponent(this.PosComp);

            this.game = game;
            this.SpriteComp.Sprite = sprite;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteComp.Draw(spriteBatch);
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
    }
}
