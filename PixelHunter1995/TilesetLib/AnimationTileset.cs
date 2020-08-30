using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelHunter1995.TilesetLib
{
    class AnimationTileset : Tileset
    {
        // Using hard coded values asuming all animation tilesets look the same
        private static readonly int IMAGE_WIDTH = 999; // value currently not used...
        private static readonly int IMAGE_HEIGHT = 999; // value currently not used...
        private static readonly int FIRST_GID = 0;
        private static readonly int TILE_WIDTH = 32;
        private static readonly int TILE_HEIGHT = 48;
        private static readonly string NAME = "dummy_name"; // value currently not used...
        private static readonly int TILE_COUNT = 16;
        private static readonly int NO_OF_COLUMNS = 4;

        // GID referencing first tile
        private static readonly int STAND_DOWN = 0;
        private static readonly int STAND_UP = 1;
        private static readonly int STAND_RIGHT = 2;
        private static readonly int MOVE_DOWN = 8;
        private static readonly int MOVE_UP = 12;
        private static readonly int MOVE_RIGHT = 4;

        // How fast to change between animation steps
        private static readonly int FRAMES_PER_ANIMATION_STEP = 8; // TODO: Use real time instead of hard coding

        enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        int FrameCounter;
        int CurrentAnimationStep;
        Direction CurrentDirection;
        bool AnimationReversing;

        public AnimationTileset(string imagePath)
            : base(imagePath, IMAGE_WIDTH, IMAGE_HEIGHT, FIRST_GID, NAME,
                  TILE_WIDTH, TILE_HEIGHT, TILE_COUNT, NO_OF_COLUMNS)
        {
            CurrentAnimationStep = 0;
            FrameCounter = 0;
            CurrentDirection = Direction.Down;
            AnimationReversing = false;
        }

        private void DrawStanding(SpriteBatch spriteBatch, Vector2 destination, double scaling)
        {
            switch (CurrentDirection)
            {
                case Direction.Left:
                    Draw(spriteBatch, destination, STAND_RIGHT, scaling, SpriteEffects.FlipHorizontally);
                    break;
                case Direction.Right:
                    Draw(spriteBatch, destination, STAND_RIGHT, scaling);
                    break;
                case Direction.Up:
                    Draw(spriteBatch, destination, STAND_UP, scaling);
                    break;
                case Direction.Down:
                    Draw(spriteBatch, destination, STAND_DOWN, scaling);
                    break;
            }

            // Reset to start animation from beginning when we move next time
            CurrentAnimationStep = 0;
            FrameCounter = 0;
            AnimationReversing = false;
        }

        private void DrawMoving(SpriteBatch spriteBatch, Vector2 destination, double scaling)
        {
            switch (CurrentDirection)
            {
                case Direction.Left:
                    Draw(spriteBatch, destination, MOVE_RIGHT + CurrentAnimationStep, scaling, SpriteEffects.FlipHorizontally);
                    break;
                case Direction.Right:
                    Draw(spriteBatch, destination, MOVE_RIGHT + CurrentAnimationStep, scaling);
                    break;
                case Direction.Up:
                    Draw(spriteBatch, destination, MOVE_UP + CurrentAnimationStep, scaling);
                    break;
                case Direction.Down:
                    Draw(spriteBatch, destination, MOVE_DOWN + CurrentAnimationStep, scaling);
                    break;
            }

            // Change animation frame
            FrameCounter++;
            if (FrameCounter == FRAMES_PER_ANIMATION_STEP)
            {
                FrameCounter = 0;
                CurrentAnimationStep = AnimationReversing ? --CurrentAnimationStep : ++CurrentAnimationStep;
                if (CurrentAnimationStep == 0)
                {
                    AnimationReversing = false;
                }
                else if (CurrentAnimationStep == (NO_OF_COLUMNS - 1))
                {
                    AnimationReversing = true;
                }
            }
        }

        private void SetCurrentDirection(Vector2 moveDirection)
        {
            if (Math.Abs(moveDirection.X) > Math.Abs(moveDirection.Y))
            {
                if (moveDirection.X > 0)
                {
                    CurrentDirection = Direction.Right;
                }
                else
                {
                    CurrentDirection = Direction.Left;
                }
            }
            else
            {
                if (moveDirection.Y > 0)
                {
                    CurrentDirection = Direction.Down;
                }
                else
                {
                    CurrentDirection = Direction.Up;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 destination, Vector2 moveDirection, double scaling)
        {
            if (moveDirection.LengthSquared() == 0)
            {
                DrawStanding(spriteBatch, destination, scaling);
            }
            else
            {
                SetCurrentDirection(moveDirection);
                DrawMoving(spriteBatch, destination, scaling);
            }

        }

    }
}
