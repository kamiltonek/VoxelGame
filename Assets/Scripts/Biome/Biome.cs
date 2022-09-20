using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;
using System;

public abstract class Biome
{
    public virtual float layerIncrement { get { return 0.045f; } }
    public virtual float waterLayerY { get { return 4; } }
    public virtual int minHeight { get { return 4; } }
    public virtual int maxHeight { get { return 9; } }

    protected float generatedY;
    // Parametr okreœlaj¹cy odleg³oœæ od zbiorników wodnych
    // (-) woda
    // (+) l¹d
    private float waterDistance { get; }

    protected Biome(float waterDistance)
    {
        this.waterDistance = waterDistance;
    }


    public virtual BlockType GenerateTerrain(float x, float y, float z)
    {
        GenerateTerrainValues(x, z);


        if (y == generatedY)
        {
            return GenerateSurface();
        }

        if(y < generatedY)
        {
            return Generate1stLayer();
        }

        if(y <= waterLayerY)
        {
            return GenerateWaterLayer();
        }

        return World.blockTypes[BlockNameEnum.AIR];
    }

    protected virtual BlockType GenerateSurface()
    {
        return World.blockTypes[BlockNameEnum.GRASS];
    }

    protected virtual BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockNameEnum.DIRT];
    }

    protected virtual BlockType GenerateWaterLayer()
    {
        return World.blockTypes[BlockNameEnum.WATER];
    }

    protected virtual void GenerateTerrainValues(float x, float z)
    {
        generatedY =
            (float)(Math.Round(
                (decimal)2 * (decimal)ChunkUtils.GenerateHeight(x, z, layerIncrement, minHeight, maxHeight, waterDistance),
                MidpointRounding.AwayFromZero) / 2);
    }

    public virtual BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.UNDEFINED;
    }
}
