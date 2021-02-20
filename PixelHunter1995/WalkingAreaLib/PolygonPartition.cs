using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace PixelHunter1995.WalkingAreaLib
{
    class PolygonPartition
    {
        private Polygon OriginalPolygon;
        public List<Polygon> Polygons { get; }
        public List<List<int>> Adjacents { get; }
        public float[,] DistanceMatrix { get; }

        public PolygonPartition(List<Polygon> polygons, Polygon originalPolygon)
        {
            OriginalPolygon = originalPolygon;
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
                        return new PolygonPartition(newPolygons, OriginalPolygon).RemoveUnnecessaryEdges();
                    }
                }
            }

            return new PolygonPartition(newPolygons, OriginalPolygon);
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

        public bool ContainsLine(Vector2 pointA, Vector2 pointB)
        {
            if (!Contains(pointA) || !Contains(pointB))
            {
                return false;
            }

            List<Vector2> intersections = OriginalPolygon.EdgeIntersections(pointA, pointB);
            foreach (Vector2 intersectionA in intersections)
            {
                foreach (Vector2 intersectionB in intersections)
                {
                    if (intersectionA.Equals(intersectionB))
                    {
                        continue;
                    }

                    Vector2 pointAToB = intersectionB - intersectionA;
                    Vector2 midwayPoint = intersectionA + new Vector2(pointAToB.X / 2.0f, pointAToB.Y / 2.0f);
                    if (!Contains(midwayPoint))
                    {
                        return false;
                    }
                }
            }

            return true;
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
