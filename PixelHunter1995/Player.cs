﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Components;
using PixelHunter1995.TilesetLib;
using PixelHunter1995.Inputs;
using System;
using PixelHunter1995.WalkingAreaLib;
using PixelHunter1995.Utilities;

namespace PixelHunter1995
{

    class Player : IDrawable, ILoadContent, IHasComponent<PositionComponent>, IHasComponent<CharacterComponent>, ICharacterComponent
    {
        private Vector2 MovePosition { get; set; }
        private Vector2 LastClickedPosition { get; set; }

        private PositionComponent PosComp { get; set; }
        private CharacterComponent CharComp { get; set; }

        // alias
        public Vector2 Position { get => this.PosComp.Position; private set => this.PosComp.Position = value; }
        public Vector2 MoveDirection { get => this.CharComp.MoveDirection; set => this.CharComp.MoveDirection = value; }
        public Color FontColor { get => this.CharComp.FontColor; set => this.CharComp.FontColor = value; }
        public String FontName { get => this.CharComp.FontName; set => this.CharComp.FontName = value; }
        public Vector2 FeetPosition { get => CharComp.FeetPosition; }
        public string Name { get; }

        public AnimationTileset AnimationTileset
        {
            get => this.CharComp.AnimationTileset;
            set => this.CharComp.AnimationTileset = value;
        }

        PositionComponent IHasComponent<PositionComponent>.Component => PosComp;
        CharacterComponent IHasComponent<CharacterComponent>.Component => CharComp;

        public Player(string name) : this(50, 50, name) { }
        public Player(float x, float y, string name)
        {
            this.PosComp = new PositionComponent();
            this.CharComp = new CharacterComponent(this.PosComp);

            this.FontColor = Color.Purple;
            this.FontName = "Alkhemikal";
            this.AnimationTileset = new AnimationTileset("Animations/felixia");
            this.Name = name;

            this.Position = new Vector2(x, y);
            this.MovePosition = this.Position;
            this.LastClickedPosition = GetFeetPosition();
            this.MoveDirection = new Vector2();
        }

        /// <summary>
        /// Input center position of feet and get position of AnimationTilset
        /// </summary>
        private Vector2 GetPositionFromFeet(Vector2 feetPosition)
        {
            return new Vector2(feetPosition.X - AnimationTileset.tileWidth / 2, feetPosition.Y - AnimationTileset.tileHeight);
        }

        /// <summary>
        /// Get position of feet
        /// </summary>
        private Vector2 GetFeetPosition()
        {
            return new Vector2(Position.X + AnimationTileset.tileWidth / 2, Position.Y + AnimationTileset.tileHeight);
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
            if (input.GetState(InputCommand.EXPLORING_Move).IsEdgeDown)
            {
                float x = input.MouseSceneX;
                float y = input.MouseSceneY;
                // Check if position is in Scene (and not inventory etc)
                if (0 < y && y < GlobalSettings.SCENE_HEIGHT)
                {
                    LastClickedPosition = new Vector2(x, y);
                }
            }
            WalkingArea currentWalkingArea = GameManager.Instance.SceneManager.currentScene.WalkingArea;
            MovePosition = GetPositionFromFeet(currentWalkingArea.GetNextPosition(LastClickedPosition, GetFeetPosition()));
            MoveDirection = MovePosition - Position;
            Position = Approach(Position, MovePosition, 2);
        }

        public void Say(string speech)
        {
            CharComp.Say(speech);
        }

        private double Scaling(double scalingMin)
        {
            double scaling = scalingMin + (1.0 - scalingMin) * (FeetPosition.Y / GlobalSettings.SCENE_HEIGHT);
            // Make scaling happen in more discrete steps.
            return Math.Round(scaling / 0.1) * 0.1;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scalingMin)
        {
            CharComp.Draw(graphics, spriteBatch, Scaling(scalingMin));
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

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
            MovePosition = Position;
        }
    }
}
