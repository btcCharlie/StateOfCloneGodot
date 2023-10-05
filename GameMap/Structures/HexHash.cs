using Godot;

namespace StateOfClone.GameMap
{
    /// <summary>
    /// Five-component hash value.
    /// </summary>
    public struct HexHash
    {
        public float a, b, c, d, e;

        /// <summary>
        /// Create a hex hash.
        /// </summary>
        /// <returns>Hash value based on <see cref="UnityEngine.Random"/>.</returns>
        public static HexHash Create()
        {
            HexHash hash;
            hash.a = GD.Randf() * 0.999f;
            hash.b = GD.Randf() * 0.999f;
            hash.c = GD.Randf() * 0.999f;
            hash.d = GD.Randf() * 0.999f;
            hash.e = GD.Randf() * 0.999f;
            return hash;
        }
    }
}
