using UnityEngine;

namespace Assets.Scripts.Generator
{
    public class ChunkUtils
    {
        static int moistureOffset = 0;
        static int temperatureOffset = 0;
        static int heightOffset = 0;
        static int waterNoiseOffset = 0;

        public static float GenerateHeight(float x, float z, float increment, int minHeight, int maxHeight, float waterDistance)
        {

            float generatedHeight = PerlinNoise(x * increment + heightOffset, z * increment + heightOffset);

            if(waterDistance > 0 && waterDistance < 0.05f)
            {
                generatedHeight -= Map(0, 0.5f, 1f - (waterDistance * (1f / 0.03f)));
            }
            else if(waterDistance < 0 && waterDistance > -0.05f)
            {
                generatedHeight += Map(0, 0.5f, 1f - (-waterDistance * (1f / 0.03f)));
            }

            return Map(minHeight, maxHeight, generatedHeight);
        }

        static float Map(float from, float to, float value, float perlinFrom = 0, float perlinTo = 1)
        {
            if (value <= perlinFrom)
                return from;
            if (value >= perlinTo)
                return to;

            return (to - from) * ((value - perlinFrom) / (perlinTo - perlinFrom)) + from;
        }

        public static float GenerateMoisture(float x, float z, float increment = 0.15f)
        {
            float inc1 = 0.07f;
            float inc2 = 0.25f;
            float val1 = PerlinNoise(x * inc1 + moistureOffset, z * inc1 + moistureOffset) * 0.75f;
            float val2 = PerlinNoise(x * inc2 + moistureOffset, z * inc2 + moistureOffset) * 0.25f;

            return (val1 + val2);
        }

        public static float GenerateTemperature(float x, float z, float increment = 0.15f)
        {
            float inc1 = 0.07f;
            float inc2 = 0.25f;
            float val1 = PerlinNoise(x * inc1 + temperatureOffset, z * inc1 + temperatureOffset) * 0.75f;
            float val2 = PerlinNoise(x * inc2 + temperatureOffset, z * inc2 + temperatureOffset) * 0.25f;

            return (val1 + val2);
        }

        public static float GenerateWaterNoise(float x, float z, float increment = 0.15f)
        {
            float inc1 = 0.07f;
            float inc2 = 0.25f;
            float val1 = PerlinNoise(x * inc1 + waterNoiseOffset, z * inc1 + waterNoiseOffset) * 0.75f;
            float val2 = PerlinNoise(x * inc2 + waterNoiseOffset, z * inc2 + waterNoiseOffset) * 0.25f;

            return (val1 + val2);
        }

        static float PerlinNoise(float x, float z)
        {
            float perlinValue = Mathf.PerlinNoise(x, z);
            return perlinValue;
        }

        public static void GenerateRandomOffset()
        {
            moistureOffset = Random.Range(-100000, 100000);
            temperatureOffset = Random.Range(-100000, 100000);
            heightOffset = Random.Range(-100000, 100000);
            waterNoiseOffset = Random.Range(-100000, 100000);
        }
    }
}
