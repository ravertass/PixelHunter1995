using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PixelHunter1995.WalkingAreaLib
{
    class WalkingArea : IDrawable
    {
        private PolygonPartition partition;

        private int sceneWidth;

        public WalkingArea(Polygon polygon, int sceneWidth)
        {
            partition = polygon.ConvexPartition();
            this.sceneWidth = sceneWidth;
        }

        public WalkingArea(List<Vector2> points, int sceneWidth)
            : this(new Polygon(points), sceneWidth)
        {
        }

        public override string ToString()
        {
            return partition.ToString();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scalingMin)
        {
            partition.Draw(graphics, spriteBatch, sceneWidth);
        }

        public Vector2 GetNextPosition(Vector2 clickPosition, Vector2 currentPosition)
        {
            // clickPosition outside of PolygonPartition
            if (!partition.Contains(clickPosition))
            {
                // TODO: Get closest position on edge not just closest vertex
                Vector2 closestVertex = Vector2.Zero;
                double closestDistance = double.MaxValue;
                foreach (var polygon in partition.Polygons)
                {
                    (Vector2 closest, double distance) = polygon.GetClosestVertex(clickPosition);

                    if (distance < closestDistance)
                    {
                        closestVertex = closest;
                        closestDistance = distance;
                    }
                }
                clickPosition = closestVertex;
            }
            return clickPosition;
            // TODO: Lot of smart things here, coming in next review on path finding...
        }

        public int ZIndex()
        {
            return 10;
        }
    }
}
