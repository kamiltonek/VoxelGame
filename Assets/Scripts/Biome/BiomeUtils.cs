using Assets.Scripts.Generator;
using UnityEngine;

public static class BiomeUtils
{
    public static float waterNoiseParameter = 0.1f;

    private static float worldHeight = 2160.0f;

    private static int temperatureAreaSize = 10;
    private static int moistureAreaSize = 15;
    private static int waterNoiseAreaSize = 35;

    private static Color desert = new Color(255 / 255f, 145 / 255f, 0, 255);
    private static Color grassland = new Color(141 / 255f, 179 / 255f, 96 / 255f, 255);
    private static Color savanna = new Color(189 / 255f, 178 / 255f, 95 / 255f, 255);
    private static Color snow = new Color(245 / 255f, 249 / 255f, 255 / 255f, 255);
    private static Color tajga = new Color(7 / 255f, 249 / 255f, 178 / 255f, 255);
    private static Color tundra = new Color(11 / 255f, 102 / 255f, 89 / 255f, 255);
    private static Color water = new Color(0 / 255f, 136 / 255f, 255 / 255f, 255);
    private static Color ice = new Color(122 / 255f, 173 / 255f, 255 / 255f, 255);
    private static Color river = water;//new Color(0 / 255f, 0 / 255f, 0 / 255f, 255);
    private static Color beach = new Color(250 / 255f, 222 / 255f, 85 / 255f, 255);

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


        float waterDistance = waterNoise - waterNoiseParameter;

        if (waterNoise < waterNoiseParameter)
        {
            return new Water(waterDistance);
        }

        if(temperature < -0.2)
        {
            return new Ice(waterDistance);
        }
        if(temperature < 0.10)
        {
            if (temperature > 0.8 && waterNoise < 0.5)
            {
                return new River(waterDistance);
            }
            return new Snow(waterDistance);
        }
        if(temperature < 0.66)
        {
            if (moisture < 0.5)
            {
                return new Tundra(waterDistance);
            }
            if (moisture < 0.52 && waterNoise < 0.5)
            {
                return new River(waterDistance);
            }
            return new Tajga(waterDistance);
        }
        if(temperature > 1.1)
        {
            return new Desert(waterDistance);
        }
        if(temperature > 0.9 && moisture < 0.5)
        {
            if(waterNoise < 0.33)
            {
                return new Beach(waterDistance);
            }
            return new Savanna(waterDistance);
        }
        if (waterNoise < 0.33 && moisture < 0.55)
        {
            return new Beach(waterDistance);
        }
        return new Grassland(waterDistance);

    }

    public static Color SelectBiomeColor(
        float x, 
        float z,
        float waterSpread,
        int waterAreaSize,
        float extraTemperature,
        float heightImpact,
        float biomeSize)
    {
        float temperature =
            ChunkUtils.GenerateTemperature(x / biomeSize, z / biomeSize);
        float moisture =
            ChunkUtils.GenerateMoisture(x / biomeSize, z / biomeSize);
        float waterNoise =
            ChunkUtils.GenerateWaterNoise(x / waterAreaSize, z / waterAreaSize);

        float x1 = x - (worldHeight / 2.0f);
        float x2 = x1 / (worldHeight / 2.0f);
        float x3 = x2 * Mathf.PI / 2.0f;
        float x4 = heightImpact * Mathf.Sin(x3);

        temperature -= x4 - extraTemperature;


        if (waterNoise < waterSpread)
        {
            return water;
        }

        if (temperature < -0.2)
        {
            return ice;
        }
        if (temperature < 0.10)
        {
            if (temperature > 0.8 && waterNoise < 0.5)
            {
                return river;
            }
            return snow;
        }
        if (temperature < 0.66)
        {
            if (moisture < 0.5)
            {
                return tundra;
            }
            if (moisture < 0.52 && waterNoise < 0.5)
            {
                return river;
            }
            return tajga;
        }
        if (temperature > 1.1)
        {
            return desert;
        }
        if (temperature > 0.9 && moisture < 0.5)
        {
            if (waterNoise < 0.33)
            {
                return beach;
            }
            return savanna;
        }
        if (waterNoise < 0.33 && moisture < 0.55)
        {
            return beach;
        }
        return grassland;


    }

    private static Color temp1 = new Color(228 / 255f, 134 / 255f, 37 / 255f, 255);
    private static Color temp2 = new Color(240 / 255f, 174 / 255f, 53 / 255f, 255);
    private static Color temp3 = new Color(249 / 255f, 218 / 255f, 70 / 255f, 255);
    private static Color temp4 = new Color(165 / 255f, 224 / 255f, 54 / 255f, 255);
    private static Color temp5 = new Color(83 / 255f, 197 / 255f, 116 / 255f, 255);
    private static Color temp6 = new Color(220 / 255f, 239 / 255f, 255 / 255f, 255);
    private static Color temp7 = new Color(171 / 255f, 216 / 255f, 255 / 255f, 255);
    private static Color temp8 = new Color(85 / 255f, 175 / 255f, 255 / 255f, 255);


    public static Color SelectTemperature(
        float x,
        float z,
        float size)
    {
        float temperature =
            ChunkUtils.GenerateTemperature(x / size, z / size);

        if(temperature < 0.125f)
        {
            return temp8;
        }

        if (temperature < 0.25f)
        {
            return temp7;
        }

        if (temperature < 0.375f)
        {
            return temp6;
        }

        if (temperature < 0.5)
        {
            return temp5;
        }

        if (temperature < 0.625f)
        {
            return temp4;
        }

        if (temperature < 0.75f)
        {
            return temp3;
        }

        if (temperature < 0.875f)
        {
            return temp2;
        }

        return temp1;
    }

    private static Color moisture1 = new Color(147 / 255f, 228 / 255f, 249 / 255f, 255);
    private static Color moisture2 = new Color(130 / 255f, 195 / 255f, 251 / 255f, 255);
    private static Color moisture3 = new Color(101 / 255f, 102 / 255f, 247 / 255f, 255);
    private static Color moisture4 = new Color(197 / 255f, 152 / 255f, 244 / 255f, 255);
    private static Color moisture5 = new Color(115 / 255f, 61 / 255f, 155 / 255f, 255);


    public static Color SelectMoisture(
        float x,
        float z,
        float size)
    {
        float temperature =
            ChunkUtils.GenerateMoisture(x / size, z / size);

        if (temperature < 0.2f)
        {
            return moisture1;
        }

        if (temperature < 0.4f)
        {
            return moisture2;
        }

        if (temperature < 0.6f)
        {
            return moisture3;
        }

        if (temperature < 0.8)
        {
            return moisture1;
        }

        return moisture5;
    }

    public static Color SelectWater(
        float x,
        float z,
        float size)
    {
        float temperature =
            ChunkUtils.GenerateMoisture(x / size, z / size);

        if (temperature < 0.5f)
        {
            return water;
        }

        return grassland;
    }
}
