using Godot;
using System.Collections.Generic;
using StateOfClone.GameMap;

public partial class MapMesh : MeshInstance3D
{
    private int _rings = 50;
    private int _radialSegments = 50;
    private float _radius = 1f;

    private List<Vector3> _vertices = new();
    private List<Vector2> _uvs = new();
    private List<Vector3> _normals = new();
    private List<int> _indices = new();
    private List<float> _custom0 = new();

    public override void _Ready()
    {
        Godot.Collections.Array surfaceArray = new();
        surfaceArray.Resize((int)Mesh.ArrayType.Max);

        CreateGeometry();

        surfaceArray[(int)Mesh.ArrayType.Vertex] = _vertices.ToArray();
        surfaceArray[(int)Mesh.ArrayType.TexUV] = _uvs.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Normal] = _normals.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Index] = _indices.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Custom0] = _custom0.ToArray();

        if (Mesh is ArrayMesh arrMesh)
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
    }

    private void CreateGeometry()
    {



        // Vertex indices.
        var thisRow = 0;
        var prevRow = 0;
        var point = 0;

        // Loop over rings.
        for (var i = 0; i < _rings + 1; i++)
        {
            var v = ((float)i) / _rings;
            var w = Mathf.Sin(Mathf.Pi * v);
            var y = Mathf.Cos(Mathf.Pi * v);

            // Loop over segments in ring.
            for (int j = 0; j < _radialSegments; j++)
            {
                float ringRatio = (float)j / _radialSegments * Mathf.Pi;
                float u = ((float)j) / _radialSegments;
                float x = Mathf.Sin(u * Mathf.Pi * 2);
                var z = Mathf.Cos(u * Mathf.Pi * 2);
                var vert = new Vector3(x * _radius * w, y * _radius, z * _radius * w);
                _vertices.Add(vert);
                _normals.Add(vert.Normalized());
                _custom0.Add(1 - Mathf.Sin(ringRatio));
                _custom0.Add(Mathf.Sin(ringRatio));
                _custom0.Add(0f);
                _custom0.Add(1f);
                _uvs.Add(new Vector2(u, v));
                point += 1;

                // Create triangles in ring using indices.
                if (i > 0 && j > 0)
                {
                    _indices.Add(prevRow + j - 1);
                    _indices.Add(prevRow + j);
                    _indices.Add(thisRow + j - 1);

                    _indices.Add(prevRow + j);
                    _indices.Add(thisRow + j);
                    _indices.Add(thisRow + j - 1);
                }
            }

            if (i > 0)
            {
                _indices.Add(prevRow + _radialSegments - 1);
                _indices.Add(prevRow);
                _indices.Add(thisRow + _radialSegments - 1);

                _indices.Add(prevRow);
                _indices.Add(prevRow + _radialSegments);
                _indices.Add(thisRow + _radialSegments - 1);
            }

            prevRow = thisRow;
            thisRow = point;
        }
    }


    public override void _Process(double delta)
    {
    }
}