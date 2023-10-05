using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateOfClone.Core
{
    public static class MeshUtils
    {
        /// <summary>
        /// Merges vertices in the given mesh that occupy the same position within the specified tolerance.
        /// Returns a new mesh with the merged vertices and reduced number of vertices.
        /// </summary>
        /// <param name="mesh">The mesh to merge vertices in.</param>
        /// <param name="tolerance">The tolerance within which vertices are considered to occupy the same position.</param>
        /// <returns>A new mesh with merged vertices.</returns>
        public static Mesh MergeVertices(Mesh mesh, float tolerance = 0.001f)
        {
            // TODO: look into MeshData if it can 
            Godot.Collections.Array<Vector3> verticesArr =
                new(mesh.SurfaceGetArrays((int)ArrayMesh.ArrayType.Vertex));
            Godot.Collections.Array<int> trianglesArr =
                new(mesh.SurfaceGetArrays((int)ArrayMesh.ArrayType.Index));

            Vector3[] vertices = verticesArr.ToArray();
            int[] triangles = trianglesArr.ToArray();

            // Merge the vertices
            Vector3[] mergedVertices = MergeVertices(vertices, tolerance);

            // Map the original vertex indices to the merged vertex indices
            int[] vertexMap = MapVertices(vertices, mergedVertices, tolerance);

            // Create a new array of triangles with the merged vertices
            int[] newTriangles = MapTriangles(triangles, vertexMap);

            // Create a new mesh with the merged vertices and triangles
            Mesh newMesh = new();

            Godot.Collections.Array surfaceArray = new();
            surfaceArray.Resize((int)Mesh.ArrayType.Max);

            surfaceArray[(int)Mesh.ArrayType.Vertex] = mergedVertices;
            // surfaceArray[(int)Mesh.ArrayType.TexUV] = _uvs.ToArray();
            // surfaceArray[(int)Mesh.ArrayType.Normal] = _normals.ToArray();
            surfaceArray[(int)Mesh.ArrayType.Index] = newTriangles;
            // surfaceArray[(int)Mesh.ArrayType.Custom0] = _custom0.ToArray();

            if (newMesh is ArrayMesh arrMesh)
            {
                Mesh.ArrayFormat flags = (Mesh.ArrayFormat)(
                    (int)Mesh.ArrayCustomFormat.RgbaFloat <<
                    (int)Mesh.ArrayFormat.FormatCustom0Shift
                    );

                // Create mesh surface from mesh array
                // No blendshapes, lods, or compression used.
                arrMesh.AddSurfaceFromArrays(
                    Mesh.PrimitiveType.Triangles, surfaceArray, flags: flags
                    );
            }

            // Debug.Log($"Mesh vertex count reduced from {vertices.Length} to {mergedVertices.Length} vertices. Reduction of {(float)mergedVertices.Length / vertices.Length:P2}");

            return newMesh;
        }

        private static Vector3[] MergeVertices(Vector3[] vertices, float tolerance)
        {
            var hash = new Dictionary<Vector3, int>();
            var mergedVertices = new List<Vector3>(vertices.Length);

            for (int originalVertexIndex = 0; originalVertexIndex < vertices.Length; originalVertexIndex++)
            {
                var originalVertex = vertices[originalVertexIndex];
                var roundedVertex = new Vector3(
                    Mathf.Round(originalVertex.X / tolerance) * tolerance,
                    Mathf.Round(originalVertex.Y / tolerance) * tolerance,
                    Mathf.Round(originalVertex.Z / tolerance) * tolerance
                    );

                if (hash.TryGetValue(roundedVertex, out int mergedVertexIndex))
                {
                    // Vertex is already merged
                }
                else
                {
                    mergedVertexIndex = mergedVertices.Count;
                    mergedVertices.Add(originalVertex);
                    hash[roundedVertex] = mergedVertexIndex;
                }
            }

            return mergedVertices.ToArray();
        }

        private static int[] MapVertices(Vector3[] originalVertices, Vector3[] mergedVertices, float tolerance)
        {
            var hash = new Dictionary<Vector3, int>();
            var vertexMap = new int[originalVertices.Length];

            for (int i = 0; i < mergedVertices.Length; i++)
            {
                var mergedVertex = mergedVertices[i];
                var roundedVertex = new Vector3(Mathf.Round(mergedVertex.X / tolerance) * tolerance,
                                                Mathf.Round(mergedVertex.Y / tolerance) * tolerance,
                                                Mathf.Round(mergedVertex.Z / tolerance) * tolerance);

                hash[roundedVertex] = i;
            }

            for (int i = 0; i < originalVertices.Length; i++)
            {
                var originalVertex = originalVertices[i];
                var roundedVertex = new Vector3(Mathf.Round(originalVertex.X / tolerance) * tolerance,
                                                Mathf.Round(originalVertex.Y / tolerance) * tolerance,
                                                Mathf.Round(originalVertex.Z / tolerance) * tolerance);

                vertexMap[i] = hash[roundedVertex];
            }

            return vertexMap;
        }

        private static int[] MapTriangles(int[] triangles, int[] vertexMap)
        {
            var newTriangles = new int[triangles.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                newTriangles[i] = vertexMap[triangles[i]];
            }

            return newTriangles;
        }
    }
}
