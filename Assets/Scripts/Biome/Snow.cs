using Assets.Scripts;
using Assets.Scripts.Enums;

public class Snow : Biome
{
    public Snow(float waterDistance) : base(waterDistance)
    {
    }

    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.SNOW_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.SNOW_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.SNOW_BIOME;
    }
}
