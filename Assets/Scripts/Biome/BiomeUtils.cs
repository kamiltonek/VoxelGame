using Assets.Scripts.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BiomeUtils
{
    private static int biomeSize = 15;
    public static Biome SelectBiome(float x, float z)
    {
        float temperature = ChunkUtils.GenerateTemperature(x / biomeSize, z / biomeSize);
        float moisture = ChunkUtils.GenerateMoisture(x / biomeSize, z / biomeSize);

        Biome biome;

        if (temperature < 0.25f)
        {
            biome = new Snow();
        }
        else if (moisture < 0.4f && temperature > 0.7f)
        {
            biome = new Desert();
        }
        else
        {
            biome = new StandardBiome();
        }

        return biome;
    }
}
