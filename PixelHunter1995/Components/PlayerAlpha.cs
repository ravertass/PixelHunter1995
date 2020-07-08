using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Components;
using System;

namespace PixelHunter1995.Components
{
    static class Utils
    {
        public static TComponent GetComponent<TComponent>(this IHasComponent<TComponent> owner)
            where TComponent : IComponent
        {
            return owner.Component;
        }
    }

    class PlayerAlpha : Player, IUpdateable, IDrawable, IHasComponent<PositionComponent>, IHasComponent<SpriteComponent>, IHasComponents
    {
        Vector2 MovePosition { get; set; }

        //PositionComponent IHasComponentGamma<PositionComponent>.Component => new PositionComponent();
        //SpriteComponent IHasComponentGamma<SpriteComponent>.Component => new SpriteComponent();
        /*
        private PositionComponent PosComp { get; set; }
        private SpriteComponent SpriteComp { get; set; }
        PositionComponent IHasComponentGamma<PositionComponent>.Component { get => this.PosComp; set => this.PosComp = value; }
        SpriteComponent IHasComponentGamma<SpriteComponent>.Component { get => this.SpriteComp; set => this.SpriteComp = value; }
        */
        PositionComponent IHasComponent<PositionComponent>.Component { get; set; }
        SpriteComponent IHasComponent<SpriteComponent>.Component { get; set; }
        private PositionComponent PosComp { get => ((IHasComponent<PositionComponent>)this).Component; }
        private SpriteComponent SpriteComp { get => ((IHasComponent<SpriteComponent>)this).Component; }

        private Vector2 Position
        {
            get => PosComp.Position;
            set => PosComp.Position = value;
        }

        private readonly Game game;

        public PlayerAlpha(Game game)
        {
            //*
            ((IHasComponent<PositionComponent>)this).Component = new PositionComponent();
            //((IHasComponentGamma<SpriteComponent>)this).Component = (SpriteComponent)new SpriteComponent(this).SetOwner(this);
            ((IHasComponent<SpriteComponent>)this).Component = new SpriteComponent().SetOwner(this);
            //*/

            /* might be fun playing with, ultimately unwanted tho. Does not give compile-time errors.
            foreach (Type tinterface in (this.GetType().GetInterfaces()))
            {
                if (tinterface.IsGenericType && tinterface.GetGenericTypeDefinition() == typeof(IHasComponentGamma<>))
                {
                    Console.WriteLine("interface - " + tinterface.ToString());
                    Console.WriteLine("  " + tinterface.GetGenericTypeDefinition());
                    Console.WriteLine("  " + tinterface.GenericTypeArguments.Length);
                    Console.WriteLine("  " + tinterface.GenericTypeArguments[0]);
                    var component = Activator.CreateInstance(tinterface.GenericTypeArguments[0]);
                    var prop = tinterface.GetProperty("Component");
                    var method = tinterface.GenericTypeArguments[0].GetMethod("SetOwner");
                    var component2 = method.MakeGenericMethod(this.GetType()).Invoke(component, new object[] { this });
                    prop.SetValue(this, component2);
                }
            }
            //*/

            //PosComp = new PositionComponent();
            //SpriteComp = new SpriteComponent(this);
            //this.GetComponent<PositionComponent>().SetOwner(this);
            //SpriteComp.SetOwner(this);

            this.game = game;

            this.Position = new Vector2(50, 50);
            this.MovePosition = this.Position;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent(ContentManager content)
        {
            SpriteComp.Sprite = content.Load<Texture2D>("Images/snubbe");
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (game.IsActive && mouseState.LeftButton == ButtonState.Pressed)
            {
                this.MovePosition = new Vector2(mouseState.X, mouseState.Y);
            }

            this.Position = this.Approach(Position, MovePosition, 2);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteComp.Draw(spriteBatch);
            //spriteBatch.Draw(SpriteComp.Sprite, Position, Color.White);
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
