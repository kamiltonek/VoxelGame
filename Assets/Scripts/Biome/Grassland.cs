using Assets.Scripts;
using Assets.Scripts.Enums;

public class Grassland : Biome
{
    public Grassland(float waterDistance) : base(waterDistance)
    {
    }

    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.GRASSLAND_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.GRASSLAND_BLOCK];
    }
    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.GRASSLAND;
    }
}
