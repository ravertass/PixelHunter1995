using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.WalkingAreaLib;

namespace PixelHunter1995.SceneLib
{
    public enum ExitDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public class Portal : IDrawable
    {
        public readonly Vector2 Position;
        private readonly Polygon Polygon;

        private readonly int SceneWidth;
        private readonly int SceneHeight;

        public readonly ExitDirection ExitDirection;

        public string Name { get; private set; }
        public string DestinationScene { get; private set; }
        public string DestinationPortal { get; private set; }
        // TODO: This should probably be set through Tiled properties.
        public Vector2 AppearancePosition { get => Position; }

        public Portal(Vector2 position,
                      List<Vector2> points,
                      int sceneWidth,
                      int sceneHeight,
                      string name,
                      string destinationScene,
                      string destinationPortal,
                      ExitDirection exitDirection)
        {
            Position = position;
            SceneWidth = sceneWidth;
            SceneHeight = sceneHeight;
            Polygon = new Polygon(points);
            Name = name;
            DestinationScene = destinationScene;
            DestinationPortal = destinationPortal;
            ExitDirection = exitDirection;
        }

        public Vector2 ExitPosition
        {
            get
            {
                int outsideScreenOffset = 64;

                if (ExitDirection == ExitDirection.Left)
                {
                    return new Vector2(-outsideScreenOffset, Position.Y);
                }
                else if (ExitDirection == ExitDirection.Right)
                {
                    return new Vector2(SceneWidth + outsideScreenOffset, Position.Y);
                }
                else if (ExitDirection == ExitDirection.Up)
                {
                    return new Vector2(Position.X, -outsideScreenOffset);
                }
                else if (ExitDirection == ExitDirection.Down)
                {
                    return new Vector2(Position.X, SceneHeight + outsideScreenOffset);
                }
                else
                {
                    return Position;
                }
            }
        }

        internal string HoverText()
        {
            if (ExitDirection == ExitDirection.Left)
            {
                return "Go left";
            }
            else if (ExitDirection == ExitDirection.Right)
            {
                return "Go right";
            }
            else if (ExitDirection == ExitDirection.Up)
            {
                return "Go up";
            }
            else if (ExitDirection == ExitDirection.Down)
            {
                return "Go down";
            }
            else
            {
                return "Go somewhere!?";
            }
        }

        public bool Contains(Vector2 point)
        {
            return Polygon.Contains(point);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            Polygon.Draw(graphics, spriteBatch, scaling, SceneWidth);
        }

        public int ZIndex()
        {
            return 10;
        }

        internal ExitDirection ReverseExitDirection()
        {
            if (ExitDirection == ExitDirection.Left)
            {
                return ExitDirection.Right;
            }
            else if (ExitDirection == ExitDirection.Right)
            {
                return ExitDirection.Left;
            }
            else if (ExitDirection == ExitDirection.Up)
            {
                return ExitDirection.Down;
            }
            else
            {
                return ExitDirection.Up;
            }
        }
    }
}
