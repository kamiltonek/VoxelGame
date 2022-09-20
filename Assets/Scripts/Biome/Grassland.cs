using Assets.Scripts;
using Assets.Scripts.Enums;

public class Grassland : Biome
{
    public Grassland(float waterDistance) : base(waterDistance)
    {
    }

    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockNameEnum.GRASSLAND_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockNameEnum.DIRT];
    }
    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.GRASSLAND;
    }
}
