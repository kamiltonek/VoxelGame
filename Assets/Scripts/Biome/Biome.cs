using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Biome
{
    public virtual float layerIncrement { get { return 0.02f; } }

    protected int generatedY;

    public virtual BlockType GenerateTerrain(float x, float y, float z)
    {
        GenerateTerrainValues(x, z);

        if(y * 2 == generatedY)
        {
            return GenerateSurface();
        }

        if(y * 2 < generatedY)
        {
            return Generate1stLayer();
        }

        return World.blockTypes[BlockName.AIR];
    }

    protected virtual BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.GRASS];
    }

    protected virtual BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.DIRT];
    }

    protected virtual void GenerateTerrainValues(float x, float z)
    {
        generatedY = (int)ChunkUtils.GenerateHeight(x, z, layerIncrement);
    }
}