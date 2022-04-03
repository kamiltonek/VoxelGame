using UnityEngine;

namespace Assets.Scripts.Generator
{
    public class ChunkUtils
    {
        static int offset = 0;
        static int moistureOffset = 0;
        static int temperatureOffset = 0;
        static int waterNoiseOffset = 0;

        public static float GenerateHeight(float x, float z, float increment, int minHeight, int maxHeight, float waterDistance)
        {
            float inc1 = 0.07f;
            float inc2 = 0.25f;
            float val1 = PerlinNoise(x * inc1 + waterNoiseOffset, z * inc1 + waterNoiseOffset) * 0.75f;
            float val2 = PerlinNoise(x * inc2 + waterNoiseOffset, z * inc2 + waterNoiseOffset) * 0.25f;
            float val3 = val1 + val2;

            if(waterDistance > 0 && waterDistance < 0.05f)
            {
                val3 -= Map(0, 0.3f, 1f - (waterDistance * (1f / 0.03f)));
            }
            else if(waterDistance < 0 && waterDistance > -0.05f)
            {
                val3 += Map(0, 0.3f, 1f - (-waterDistance * (1f / 0.03f)));
            }


            //float height = PerlinNoise(x * increment + offset, z * increment + offset);

            return Map(minHeight, maxHeight, val3);
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
            float height = Mathf.PerlinNoise(x, z);
            return height;
        }

        public static void GenerateRandomOffset()
        {
            offset = Random.Range(-100000, 100000);

            moistureOffset = Random.Range(-100000, 100000);
            temperatureOffset = Random.Range(-100000, 100000);
            waterNoiseOffset = Random.Range(-100000, 100000);
            //moistureOffset = 1;
            //temperatureOffset = 214;
            //waterNoiseOffset = 523;
        }
    }
}
