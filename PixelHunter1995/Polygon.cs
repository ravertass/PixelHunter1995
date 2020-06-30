using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PixelHunter1995
{
    class Polygon : IDrawable
    {
        private List<Coord> vertices;

        public Polygon(List<Coord> vertices)
        {
            Debug.Assert(vertices.Count > 2);
            this.vertices = vertices;
        }

        public Polygon(params Coord[] vertices)
            : this(vertices.ToList())
        {
        }

        public Polygon(Polygon other)
            : this(new List<Coord>(other.vertices))
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
            Coord previous = vertices[PreviousIndex(i)];
            Coord current = vertices[i];
            Coord next = vertices[NextIndex(i)];

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

        private bool IsConvex()
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

        public List<Polygon> ConvexPartition()
        {
            // TODO: Use an actual convex partition algorithn, e.g. Hertel-Mehlhorn,
            //       instead of only using triangulation.
            if (IsConvex())
            {
                return new List<Polygon>() { this };
            }

            return Triangulate();
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
            return (((i - 1) % vertices.Count) + vertices.Count) % vertices.Count;
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
                if (new [] {PreviousIndex(i), i, NextIndex(i)}.Contains(j))
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
        /// Checks if the triangle formed by vertices at indices (i - 1, i, i + 1)
        /// contains the given point.
        /// </summary>
        private bool TriangleContainsPoint(int i, Coord point)
        {
            // Based on
            // https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle/2049593#2049593

            Coord previousVertex = vertices[PreviousIndex(i)];
            Coord currentVertex = vertices[i];
            Coord nextVertex = vertices[NextIndex(i)];

            Func<Coord, Coord, Coord, float> sign = (Coord p1, Coord p2, Coord p3) =>
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
            return String.Join<Coord>(", ", vertices);
        }

        public void Draw(GraphicsDeviceManager graphics)
        {
            if (!GlobalSettings.Instance.Debug)
            {
                return;
            }

            VertexPositionColor[] drawVertices = new VertexPositionColor[vertices.Count];
            short[] drawIndices = new short[vertices.Count * 2];
            for (int i = 0; i < vertices.Count; i++)
            {
                Coord coord = vertices[i];
                // Convert coordinates to the coordinate system
                // used when drawing primitives, with 0 in the middle of
                // the screen, X positive going right and Y positive
                // going up.
                float x = (coord.X / graphics.PreferredBackBufferWidth) * 2.0f - 1.0f;
                float y = -(coord.Y / graphics.PreferredBackBufferHeight) * 2.0f + 1.0f;
                drawVertices[i].Position = new Vector3(x, y, 0.0f);
                drawIndices[i * 2] = (short)i;
                drawIndices[i * 2 + 1] = (short)((i + 1) % vertices.Count);
            }

            int vertOffset = 0;
            short indexOffset = 0;

            // Apparently, we need to use a shader for this.
            BasicEffect basicEffect = new BasicEffect(graphics.GraphicsDevice);
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
        }
    }
}
