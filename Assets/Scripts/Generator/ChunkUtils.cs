using UnityEngine;

namespace Assets.Scripts.Generator
{
    public class ChunkUtils
    {
        static int offset = 0;
        static int maxHeight = 8;
        static float increment = 0.065f;

        public static float GenerateHeight(float x, float z)
        {
            float height = PerlinNoise(x * increment + offset, z * increment + offset);

            return Map(1, maxHeight, height);
        }

        static float Map(float from, float to, float value, float perlinFrom = 0, float perlinTo = 1)
        {
            if (value <= perlinFrom)
                return from;
            if (value >= perlinTo)
                return to;

            return (to - from) * ((value - perlinFrom) / (perlinTo - perlinFrom)) + from;
        }

        static float PerlinNoise(float x, float z)
        {
            float height = Mathf.PerlinNoise(x, z);
            return height;
        }

        public static void GenerateRandomOffset()
        {
            offset = Random.Range(0, 1000);
        }
    }
}
