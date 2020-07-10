using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Components;
using System;

namespace PixelHunter1995.Components.Gamma
{

    class PlayerGamma : IPlayer, IUpdateable, IDrawable, IHasComponentsGamma
    {
        Vector2 MovePosition { get; set; }

        private PositionComponentGamma PosComp { get; }
        private SpriteComponentGamma SpriteComp { get; }

        private Vector2 Position
        {
            get => PosComp.Position;
            set => PosComp.Position = value;
        }

        public CompositeGamma Composite { get; }

        private readonly Game game;

        public PlayerGamma(Game game)
        {
            Composite = new CompositeGamma();

            /*
            Composite
                .AddComponent(PosComp = new PositionComponentGamma())
                .AddComponent(SpriteComp = new SpriteComponentGamma());
            //*/

            PosComp = Composite.AddComponent<PositionComponentGamma>();
            SpriteComp = Composite.AddComponent<SpriteComponentGamma>();

            Composite.Init();

            this.game = game;

            this.Position = new Vector2(0, 0);
            this.MovePosition = this.Position;
        }

        public PlayerGamma(Game game, float x, float y) : this(game)
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
