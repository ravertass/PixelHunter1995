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

        private Vector2 ClosestPositionInWalkingArea(Vector2 inPosition)
        {
            if (partition.Contains(inPosition))
            {
                return inPosition;
            }

            Vector2 closestPoint = Vector2.Zero;
            double closestDistance = double.MaxValue;
            foreach (var polygon in partition.Polygons)
            {
                (Vector2 closest, double distance) = polygon.ClosestPositionInPolygon(inPosition);

                if (distance < closestDistance)
                {
                    closestPoint = closest;
                    closestDistance = distance;
                }
            }

            return closestPoint;
        }

        public Vector2 GetNextPosition(Vector2 clickPosition, Vector2 currentPosition)
        {
            clickPosition = ClosestPositionInWalkingArea(clickPosition);
            currentPosition = ClosestPositionInWalkingArea(currentPosition);

            int clickIndex = partition.ContainingPolygonIndex(clickPosition);
            int currentIndex = partition.ContainingPolygonIndex(currentPosition);

            // Case 1: There's a straight line within the walking area
            //         between the current position and the clicked position.
            if (partition.ContainsLine(currentPosition, clickPosition))
            {
                return clickPosition;
            }

            // Case 2: Find adjacent polygon in correct direction.
            int[] path = RunDijkstra(partition.Polygons.Count, partition.DistanceMatrix, currentIndex);

            int tempIndex = clickIndex;
            Stack<int> stack = new Stack<int>();
            while (!partition.Polygons[tempIndex].Contains(currentPosition))
            {
                stack.Push(tempIndex);
                tempIndex = path[tempIndex];
            }
            int nextPolygonIndex = stack.Pop();

            return partition.Polygons[nextPolygonIndex].ClosestPositionInPolygon(currentPosition).Item1;
        }

        // Taken from: https://simpledevcode.wordpress.com/2015/12/22/graphs-and-dijkstras-algorithm-c/
        // TODO:
        // - Create a matrix that counts distance in number of polygons one has to traverse.
        // - Include some library with a good license which has a priority queue to use.
        // - Implement Dijkstra ourselves...
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
