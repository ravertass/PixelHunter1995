using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Utilities;
using System;
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

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            partition.Draw(graphics, spriteBatch, scaling, sceneWidth);
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
            int currentIndex = partition.ContainingPolygonIndex(currentPosition);
            int clickIndex = partition.ContainingPolygonIndex(clickPosition);
            // Case1: In correct polygon
            if (clickIndex == currentIndex)
            {
                return clickPosition;
            }
            // Case2: In adjacent polygon
            if (partition.Adjacents[currentIndex].Contains(clickIndex))
            {
                return partition.Polygons[clickIndex].Center;
            }
            // Case3: Non adjecent Polygon, find adjacent polygon in correct direction.
            int[] path = RunDijkstra(partition.Polygons.Count, partition.DistanceMatrix, currentIndex);
            int temp = clickIndex;
            Stack<int> stack = new Stack<int>();
            while (temp != currentIndex)
            {
                stack.Push(temp);
                temp = path[temp];
            }
            return partition.Polygons[stack.Pop()].Center;
        }

        // Taken from: https://simpledevcode.wordpress.com/2015/12/22/graphs-and-dijkstras-algorithm-c/
        private int[] RunDijkstra(int graphSize, float[,] distanceMatrix, int sourceIndex)
        {
            float[] distance = new float[graphSize];
            int[] previous = new int[graphSize];
            for (int i = 0; i < graphSize; i++)
            {
                distance[i] = int.MaxValue;
                previous[i] = 0;
            }
            PriorityQueue<int> pq = new PriorityQueue<int>();
            //enqueue the source
            distance[sourceIndex] = 0;
            pq.Enqueue(sourceIndex, 0);
            //insert all remaining vertices into the pq
            for (int i = 0; i < graphSize; i++)
            {
                for (int j = 0; j < graphSize; j++)
                {
                    if (distanceMatrix[i, j] > 0)
                    {
                        pq.Enqueue(i, (int)distanceMatrix[i, j]);
                    }
                }
            }
            while (!pq.Empty())
            {
                int u = pq.Dequeue_min();
                // scan each row fully
                for (int v = 0; v < graphSize; v++)
                {
                    // if there is an adjacent node
                    if (distanceMatrix[u, v] > 0)
                    {
                        float alt = distance[u] + distanceMatrix[u, v];
                        if (alt < distance[v])
                        {
                            distance[v] = alt;
                            previous[v] = u;
                            pq.Enqueue(u, (int)distance[v]);
                        }
                    }
                }
            }
            // Print distance to all Polygons for debugging
            //for (int i = 0; i < graphSize; i++)
            //{
            //    Console.WriteLine("Distance from {0} to {1}: {2}", sourceIndex, i, distance[i]);
            //}
            return previous;
        }

        public int ZIndex()
        {
            return 10;
        }
    }
}
