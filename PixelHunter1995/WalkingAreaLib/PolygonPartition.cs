using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace PixelHunter1995.WalkingAreaLib
{
    class PolygonPartition
    {
        public List<Polygon> Polygons { get; }
        public List<List<int>> Adjacents { get; }

        public PolygonPartition(List<Polygon> polygons)
        {
            Polygons = polygons;

            Adjacents = new List<List<int>>();
            for (int i = 0; i < polygons.Count; i++)
            {
                Adjacents.Add(new List<int>());
                for (int j = 0; j < polygons.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (polygons[i].HasCommonEdgeWith(polygons[j]))
                    {
                        Adjacents[i].Add(j);
                    }
                }
            }
        }

        public PolygonPartition RemoveUnnecessaryEdges()
        {
            List<Polygon> newPolygons = new List<Polygon>(Polygons);

            for (int i = 0; i < Polygons.Count; i++)
            {
                foreach (int adjacentIndex in Adjacents[i])
                {
                    if (adjacentIndex < i)
                    {
                        continue;
                    }

                    Polygon currentPolygon = Polygons[i];
                    Polygon adjacentPolygon = Polygons[adjacentIndex];
                    Polygon combined = Polygons[i].CombineWith(Polygons[adjacentIndex]);
                    if (combined.IsConvex())
                    {
                        newPolygons.Remove(currentPolygon);
                        newPolygons.Remove(adjacentPolygon);
                        newPolygons.Add(combined);
                        return new PolygonPartition(newPolygons).RemoveUnnecessaryEdges();
                    }
                }
            }

            return new PolygonPartition(newPolygons);
        }

        public override string ToString()
        {
            return string.Join<Polygon>("; ", Polygons);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling, int sceneWidth)
        {
            Polygons.ForEach(p => p.Draw(graphics, spriteBatch, scaling, sceneWidth));
        }

        public bool Contains(Vector2 position)
        {
            return Polygons.Any(polygon => polygon.Contains(position));
        }

        /// <summary>
        /// Get next position to walk towards.
        /// </summary>
        public Vector2 GetNextPosition(Vector2 clickPosition, Vector2 currentPosition)
        {
            // clickPosition outside of PolygonPartition
            if (!Contains(clickPosition))
            {
                // TODO: Get closest position on edge not just closest vertice
                Vector2 closestVertice = Vector2.Zero;
                double closestDistance = double.MaxValue;
                foreach (var polygon in Polygons)
                {
                    (Vector2 closest, double distance) = polygon.GetClosestPosition(clickPosition);

                    if (distance < closestDistance)
                    {
                        closestVertice = closest;
                        closestDistance = distance;
                    }
                }
                clickPosition = closestVertice;
            }
            return clickPosition;
            // TODO: Lot of smart things here, coming in next review on path finding...
        }

    }
}
