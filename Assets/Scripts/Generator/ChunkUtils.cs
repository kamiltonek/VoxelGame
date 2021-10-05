using UnityEngine;

namespace Assets.Scripts.Generator
{
    public class ChunkUtils
    {
        static int offset = 0;
        static int maxHeight = 10;
        static int moistureOffset = 0;
        static int temperatureOffset = 0;

        public static float GenerateHeight(float x, float z, float increment)
        {
            float height = PerlinNoise(x * increment + offset, z * increment + offset);

            return Map(0, maxHeight, height);
        }

        static float Map(float from, float to, float value, float perlinFrom = 0, float perlinTo = 1)
        {
            if (value <= perlinFrom)
                return from;
            if (value >= perlinTo)
                return to;

            return (to - from) * ((value - perlinFrom) / (perlinTo - perlinFrom)) + from;
        }

        public static float GenerateMoisture(float x, float z, float increment = 0.05f)
        {
            return PerlinNoise(x * increment + moistureOffset, z * increment + moistureOffset);
        }

        public static float GenerateTemperature(float x, float z, float increment = 0.05f)
        {
            return PerlinNoise(x * increment + temperatureOffset, z * increment + temperatureOffset);
        }

        static float PerlinNoise(float x, float z)
        {
            float height = Mathf.PerlinNoise(x, z);
            return height;
        }

        public static void GenerateRandomOffset()
        {
            //offset = Random.Range(0, 1000);
            offset = 1000;
            moistureOffset = Random.Range(0, 1000);
            temperatureOffset = Random.Range(0, 1000);
        }
    }
}
