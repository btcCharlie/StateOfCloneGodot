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
        public static HexHash Create(RandomNumberGenerator rng)
        {
            HexHash hash;
            hash.a = rng.Randf() * 0.999f;
            hash.b = rng.Randf() * 0.999f;
            hash.c = rng.Randf() * 0.999f;
            hash.d = rng.Randf() * 0.999f;
            hash.e = rng.Randf() * 0.999f;
            return hash;
        }
    }
}
