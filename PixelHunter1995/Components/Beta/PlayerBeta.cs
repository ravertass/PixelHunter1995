using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PixelHunter1995.Components.Beta
{
    class PlayerBeta : IPlayer, IUpdateable, IDrawable, IHasComponentBeta<PositionComponentBeta>, IHasComponentBeta<SpriteComponentBeta>
    {
        Vector2 MovePosition { get; set; }

        private PositionComponentBeta PosComp { get; set; }
        private SpriteComponentBeta SpriteComp { get; set; }

        // alias
        private Vector2 Position { get => this.PosComp.Position; set => this.PosComp.Position = value; }

        PositionComponentBeta IHasComponentBeta<PositionComponentBeta>.Component => PosComp;
        SpriteComponentBeta IHasComponentBeta<SpriteComponentBeta>.Component => SpriteComp;

        private readonly Game game;

        public PlayerBeta(Game game)
        {

            this.PosComp = new PositionComponentBeta();
            this.SpriteComp = new SpriteComponentBeta(this.PosComp);

            this.game = game;

            this.Position = new Vector2(0, 0);
            this.MovePosition = this.Position;
        }

        public PlayerBeta(Game game, float x, float y) : this(game)
        {
            this.Position = new Vector2(x, y);
            this.MovePosition = this.Position;
        }

        public void LoadContent(ContentManager content)
        {
            SpriteComp.Sprite = content.Load<Texture2D>("Images/snubbe");
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
            if (error.LengthSquared() <= Math.Pow(speed,2))
            {
                return target;
            }
            else
            {
                Vector2 dir = Vector2.Normalize(error);
                //return start + new Vector2(dir.X * speed, dir.Y * speed);
                return start + dir * (float) speed;
            }
        }
    }
}
