using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Generator;

public abstract class Biome
{
    public virtual float layerIncrement { get { return 0.02f; } }
    public virtual float waterLayerY { get { return 4; } }
    public virtual int minHeight { get { return 4; } }
    public virtual int maxHeight { get { return 8; } }

    protected int generatedY;
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

        if(y * 2 == generatedY)
        {
            return GenerateSurface();
        }

        if(y * 2 < generatedY)
        {
            return Generate1stLayer();
        }

        if(y * 2 <= waterLayerY)
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
        generatedY = (int)ChunkUtils.GenerateHeight(x, z, layerIncrement, minHeight, maxHeight, waterDistance);
        //generatedY = 16;
    }

    public virtual BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.UNDEFINED;
    }
}
