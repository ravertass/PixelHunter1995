using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace PixelHunter1995.WalkingAreaLib
{
    class PolygonPartition
    {
        public List<Polygon> Polygons { get; }
        public List<List<int>> Adjacents { get; }
        public float[,] DistanceMatrix { get; }

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
            DistanceMatrix = new float[Adjacents.Count, Adjacents.Count];
            for (var i = 0; i < Adjacents.Count; i++)
            {
                foreach (var j in Adjacents[i])
                {
                    DistanceMatrix[i, j] = (Polygons[i].Center - Polygons[j].Center).LengthSquared();
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

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, int sceneWidth)
        {
            Polygons.ForEach(p => p.Draw(graphics, spriteBatch, sceneWidth));
        }

        public bool Contains(Vector2 position)
        {
            return Polygons.Any(polygon => polygon.Contains(position));
        }

        public int ContainingPolygonIndex(Vector2 position)
        {
            for (int i = 0; i < Polygons.Count; i++)
            {
                if (Polygons[i].Contains(position))
                {
                    return i;
                }
            }
            throw new ArgumentException("Position " + position + " outside of PolygonPartition.");
        }
    }
}
