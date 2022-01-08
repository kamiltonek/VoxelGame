using Assets.Scripts.Generator;
using UnityEngine;

public static class BiomeUtils
{
    private static float worldHeight = 1080.0f;

    private static int temperatureAreaSize = 20;
    private static int moistureAreaSize = 15;
    private static int waterNoiseAreaSize = 35;

    public static Biome SelectBiome(float x, float z)
    {
        float temperature = 
            ChunkUtils.GenerateTemperature(x / temperatureAreaSize, z / temperatureAreaSize);
        float moisture = 
            ChunkUtils.GenerateMoisture(x / moistureAreaSize, z / moistureAreaSize);
        float waterNoise = 
            ChunkUtils.GenerateWaterNoise(x / waterNoiseAreaSize, z / waterNoiseAreaSize);

        float x1 = x - (worldHeight / 2.0f);
        float x2 = x1 / (worldHeight / 2.0f);
        float x3 = x2 * Mathf.PI / 2.0f;
        float x4 = 0.55f * Mathf.Sin(x3);

        temperature -= x4;


        if (waterNoise < 0.30)
        {
            return new Water();
        }

        if(temperature < -0.2)
        {
            return new Ice();
        }
        if(temperature < 0.10)
        {
            if (temperature > 0.8 && waterNoise < 0.5)
            {
                return new River();
            }
            return new Snow();
        }
        if(temperature < 0.66)
        {
            if (moisture < 0.5)
            {
                return new Tundra();
            }
            if (moisture < 0.52 && waterNoise < 0.5)
            {
                return new River();
            }
            return new Tajga();
        }
        if(temperature > 1.1)
        {
            return new Desert();
        }
        if(temperature > 0.9 && moisture < 0.5)
        {
            if(waterNoise < 0.33)
            {
                return new Beach();
            }
            return new Savanna();
        }
        if (waterNoise < 0.33 && moisture < 0.55)
        {
            return new Beach();
        }
        return new Grassland();


    }


}
