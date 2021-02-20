using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Utilities;
using System;

namespace PixelHunter1995.WalkingAreaLib
{
    class Polygon
    {
        private List<Vector2> vertices;
        private RenderTarget2D renderTarget = null;
        public Vector2 Center { get; }

        public Polygon(List<Vector2> vertices)
        {
            Debug.Assert(vertices.Count > 2);
            this.vertices = vertices;
            Center = vertices.Aggregate(Vector2.Zero, (acc, v) => acc + v) / vertices.Count;
        }

        ~Polygon()
        {
            if (renderTarget != null)
            {
                renderTarget.Dispose();
            }
        }

        public Polygon(params Vector2[] vertices)
            : this(vertices.ToList())
        {
        }

        public Polygon(Polygon other)
            : this(new List<Vector2>(other.vertices))
        {
        }

        enum VertexType
        {
            Convex,
            Straight,
            Concave
        }

        private VertexType GetVertexType(int i)
        {
            Vector2 previous = vertices[PreviousIndex(i)];
            Vector2 current = vertices[i];
            Vector2 next = vertices[NextIndex(i)];

            // Sorry for the stupid naming, but this was based on code
            // with even stupider naming. This probably should have good names
            // from linear algebra, if I could be bothered with doing some research.
            float a = (next.Y - previous.Y) * (current.X - previous.X);
            float b = (next.X - previous.X) * (current.Y - previous.Y);

            if (a < b)
            {
                return VertexType.Concave;
            }
            else if (a == b)
            {
                return VertexType.Straight;
            }
            else
            {
                return VertexType.Convex;
            }
        }

        /// <summary>
        /// Checks if the vertex at index i is concave, and thus makes the greater polygon concave.
        /// </summary>
        /// <param name="i">Index of the vertex to check.</param>
        /// <returns>If the vertex at index i is concave.</returns>
        private bool IsVertexConcave(int i)
        {
            return GetVertexType(i) == VertexType.Concave;
        }

        public bool IsConvex()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (IsVertexConcave(i))
                {
                    return false;
                }
            }

            return true;
        }

        public PolygonPartition ConvexPartition()
        {
            // TODO: Use an actual convex partition algorithm, e.g. Hertel-Mehlhorn,
            //       instead of only using triangulation.
            if (IsConvex())
            {
                return new PolygonPartition(new List<Polygon>() { this }, this);
            }

            return new PolygonPartition(Triangulate(), this).RemoveUnnecessaryEdges();
        }

        /// <summary>
        /// Partitions this polygon into triangles using the ear-clipping method.
        /// </summary>
        /// <returns>Partition of this polygon into triangles.</returns>
        private List<Polygon> Triangulate()
        {
            // See https://www.geometrictools.com/Documentation/TriangulationByEarClipping.pdf
            // for a good references on triangulation by ear-clipping.

            List<Polygon> ears = new List<Polygon>();

            if (vertices.Count == 3)
            {
                ears.Add(this);
                return ears;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                if (IsVertexConcave(i))
                {
                    continue;
                }

                if (IsVertexAnEar(i))
                {
                    Polygon remaining = new Polygon(this);
                    remaining.RemoveVertex(i);

                    ears = remaining.Triangulate();
                    ears.Add(VertexTriangle(i));
                    break;
                }
            }

            return ears;
        }

        private void RemoveVertex(int i)
        {
            vertices.RemoveAt(i);
        }

        private int PreviousIndex(int i)
        {
            return ((i - 1) % vertices.Count + vertices.Count) % vertices.Count;
        }

        private int NextIndex(int i)
        {
            return (i + 1) % vertices.Count;
        }

        private Polygon VertexTriangle(int i)
        {
            return new Polygon(vertices[PreviousIndex(i)], vertices[i], vertices[NextIndex(i)]);
        }

        private bool IsVertexAnEar(int i)
        {
            for (int j = 0; j < vertices.Count; j++)
            {
                if (new[] { PreviousIndex(i), i, NextIndex(i) }.Contains(j))
                {
                    continue;
                }
                if (TriangleContainsPoint(i, vertices[j]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if point is contained in the polygon.
        /// Based on https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            bool contains = false;

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 previousVertex = vertices[PreviousIndex(i)];
                Vector2 currentVertex = vertices[i];
                if ((currentVertex.Y > point.Y) != (previousVertex.Y > point.Y) &&
                    (point.X < ((previousVertex.X - currentVertex.X) * (point.Y - currentVertex.Y) /
                                (previousVertex.Y - currentVertex.Y) + currentVertex.X)))
                {
                    contains = !contains;
                }
            }

            // As can be seen when following the documentation link edge cases are not certain to be included,
            // but we need them to be (since we go there when someone clicks outside)
            if (AtPolygonEdge(point))
            {
                return true;
            }

            return contains;
        }

        private bool AtPolygonEdge(Vector2 point)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 previousVertex = vertices[PreviousIndex(i)];
                Vector2 currentVertex = vertices[i];
                if (OnLine(point, previousVertex, currentVertex))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool OnLine(Vector2 pointOfInterest, Vector2 linePointA, Vector2 linePointB)
        {
            Vector2 pointAToPOI = pointOfInterest - linePointA;
            Vector2 pointBToPOI = pointOfInterest - linePointB;
            Vector2 pointAToPointB = linePointB - linePointA;

            double lengthViaPOI = pointAToPOI.Length() + pointBToPOI.Length();
            double lineLength = pointAToPointB.Length();

            return Math.Abs(lengthViaPOI - lineLength) < 0.0000001;
        }

        /// <summary>
        /// Checks if the triangle formed by vertices at indices (i - 1, i, i + 1)
        /// contains the given point.
        /// </summary>
        private bool TriangleContainsPoint(int i, Vector2 point)
        {
            // Based on
            // https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle/2049593#2049593

            Vector2 previousVertex = vertices[PreviousIndex(i)];
            Vector2 currentVertex = vertices[i];
            Vector2 nextVertex = vertices[NextIndex(i)];

            float sign(Vector2 p1, Vector2 p2, Vector2 p3) =>
                    (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);

            float d1 = sign(point, previousVertex, currentVertex);
            float d2 = sign(point, currentVertex, nextVertex);
            float d3 = sign(point, nextVertex, previousVertex);

            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNeg && hasPos);
        }

        public override string ToString()
        {
            return string.Join<Vector2>(", ", vertices);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling, int sceneWidth)
        {
            if (!GlobalSettings.Instance.Debug)
            {
                return;
            }

            if (renderTarget == null)
            {
                renderTarget = new RenderTarget2D(
                    graphics.GraphicsDevice, sceneWidth, GlobalSettings.WINDOW_HEIGHT);
            }
            
            DrawToRenderTarget(graphics, sceneWidth);
            spriteBatch.Draw((Texture2D)renderTarget, Vector2.Zero, Color.White);
        }
        
        
        private void DrawToRenderTarget(GraphicsDeviceManager graphics, int sceneWidth)
        {
            var oldTargets = graphics.GraphicsDevice.GetRenderTargets();
            
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            graphics.GraphicsDevice.Clear(Color.Transparent);
            
            DrawPolygon(graphics, sceneWidth);
            
            graphics.GraphicsDevice.SetRenderTargets(oldTargets);
        }
        
        private void DrawPolygon(GraphicsDeviceManager graphics, int sceneWidth)
        {
            VertexPositionColor[] drawVertices = new VertexPositionColor[vertices.Count];
            short[] drawIndices = new short[vertices.Count * 2];
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 coord = vertices[i];
                // Convert coordinates to the coordinate system
                // used when drawing primitives, with 0 in the middle of
                // the screen, X positive going right and Y positive
                // going up.
                float x = (coord.X / sceneWidth) * 2.0f - 1.0f;
                float y = -(coord.Y / GlobalSettings.WINDOW_HEIGHT) * 2.0f + 1.0f;
                drawVertices[i].Position = new Vector3(x, y, 0.0f);
                drawVertices[i].Color = Color.Red;
                drawIndices[i * 2] = (short)i;
                drawIndices[i * 2 + 1] = (short)((i + 1) % vertices.Count);
            }

            int vertOffset = 0;
            short indexOffset = 0;

            // Apparently, we need to use a shader for this.
            BasicEffect basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    drawVertices,
                    vertOffset,
                    drawVertices.Length,
                    drawIndices,
                    indexOffset,
                    drawIndices.Length - 1);
            }
            basicEffect.Dispose();
        }

        public bool HasCommonEdgeWith(Polygon other)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 thisVertex = vertices[i];
                Vector2 thisNextVertex = vertices[NextIndex(i)];
                for (int j = 0; j < other.vertices.Count; j++)
                {
                    Vector2 otherVertex = other.vertices[j];
                    Vector2 otherPreviousVertex = other.vertices[other.PreviousIndex(j)];

                    if (thisVertex.Equals(otherVertex) && thisNextVertex.Equals(otherPreviousVertex))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Polygon CombineWith(Polygon other)
        {
            Debug.Assert(HasCommonEdgeWith(other));

            List<Vector2> newVertices = new List<Vector2>();
            bool collectedFromOther = false;

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 currentVertex = vertices[i];
                Vector2 nextVertex = vertices[NextIndex(i)];

                newVertices.Add(currentVertex);

                if (collectedFromOther)
                {
                    continue;
                }

                // Note: The vertex has been reversed, since it will be reversed
                //       in the other polygon.
                int otherIndex = other.EdgeAt(nextVertex, currentVertex);
                bool edgeExistsInOther = otherIndex != -1;
                if (edgeExistsInOther)
                {
                    // We reached a common edge, so we start collecting vertices from
                    // the other polygon. The correct index to start at comes after the
                    // edge (that is, the index of the first vertex in the edge plus two).
                    int startIndex = other.NextIndex(other.NextIndex(otherIndex));
                    for (int j = startIndex;
                         !vertices.Contains(other.vertices[j]);
                         j = other.NextIndex(j))
                    {
                        newVertices.Add(other.vertices[j]);
                    }
                    collectedFromOther = true;
                }
            }

            return new Polygon(newVertices);
        }

        /// <summary>
        /// Returns the index of the first vertex of this edge in the polygon,
        /// if it exists in the polygon. Otherwise, return -1.
        /// </summary>
        /// <param name="vertexA"></param>
        /// <param name="vertexB"></param>
        /// <returns></returns>
        private int EdgeAt(Vector2 vertexA, Vector2 vertexB)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 currentVertex = vertices[i];
                Vector2 nextVertex = vertices[NextIndex(i)];
                if (currentVertex.Equals(vertexA) && nextVertex.Equals(vertexB))
                {
                    return i;
                }
            }
            return -1;
        }

        private static Vector2 ProjectOntoLine(Vector2 pointOfInterest, Vector2 pointA, Vector2 pointB)
        {
            Vector2 pointAToPointB = pointB - pointA;
            Vector2 pointAToPOI = pointOfInterest - pointA;

            Vector2 normalizedPointAToPointB = pointAToPointB;
            normalizedPointAToPointB.Normalize();

            float scalarProjection = Vector2.Dot(pointAToPOI, normalizedPointAToPointB);
            Vector2 pointProjection = new Vector2(normalizedPointAToPointB.X * scalarProjection, normalizedPointAToPointB.Y * scalarProjection);

            Vector2 pointOnInfiniteLine = pointProjection + pointA;

            if (!OnLine(pointOnInfiniteLine, pointA, pointB))
            {
                if ((pointA - pointOfInterest).Length() < (pointB - pointOfInterest).Length())
                {
                    return pointA;
                }
                else
                {
                    return pointB;
                }
            }

            return pointOnInfiniteLine;
        }

        public (Vector2, double) ClosestPositionInPolygon(Vector2 pointOfInterest)
        {
            if (Contains(pointOfInterest))
            {
                return (pointOfInterest, 0.0);
            }

            Vector2 closestPoint = Vector2.Zero;
            double closestDistance = double.MaxValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 currentVertex = vertices[i];
                Vector2 nextVertex = vertices[NextIndex(i)];

                Vector2 pointOnLine = ProjectOntoLine(pointOfInterest, currentVertex, nextVertex);
                double distance = (pointOnLine - pointOfInterest).Length();
                if (distance < closestDistance)
                {
                    closestPoint = pointOnLine;
                    closestDistance = distance;
                }
            }

            return (closestPoint, closestDistance);
        }

        private static (bool, Vector2) LinesIntersect(Vector2 line1PointA,
                                                      Vector2 line1PointB,
                                                      Vector2 line2PointA,
                                                      Vector2 line2PointB)
        {
            float l1ax = line1PointA.X;
            float l1ay = line1PointA.Y;

            float l2ax = line2PointA.X;
            float l2ay = line2PointA.Y;

            Vector2 line1PointAToB = line1PointB - line1PointA;
            float l1x = line1PointAToB.X;
            float l1y = line1PointAToB.Y;

            Vector2 line2PointAToB = line2PointB - line2PointA;
            float l2x = line2PointAToB.X;
            float l2y = line2PointAToB.Y;

            float s = (-l1y * (l1ax - l2ax) + l1x * (l1ay - l2ay)) / (-l2x * l1y + l1x * l2y);
            float t = ( l2x * (l1ay - l2ay) - l2y * (l1ax - l2ax)) / (-l2x * l1y + l1x * l2y);

            bool intersects = 0 <= s && s <= 1 && 0 <= t && t <= 1;
            Vector2 intersectionPoint = new Vector2(l1ax + t * l1x, l1ay + t * l1y);

            return (intersects, intersectionPoint);
        }

        public List<Vector2> EdgeIntersections(Vector2 pointA, Vector2 pointB)
        {
            List<Vector2> intersections = new List<Vector2>();

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 currentVertex = vertices[i];
                Vector2 nextVertex = vertices[NextIndex(i)];

                var (intersects, intersectionPoint) = LinesIntersect(pointA, pointB, currentVertex, nextVertex);
                if (intersects)
                {
                    intersections.Add(intersectionPoint);
                }
            }

            return intersections;
        }
    }
}
