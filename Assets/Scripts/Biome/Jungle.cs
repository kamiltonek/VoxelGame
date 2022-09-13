using Assets.Scripts;
using Assets.Scripts.Enums;

public class Jungle : Biome
{
    public Jungle(float waterDistance) : base(waterDistance)
    {
    }

    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockNameEnum.JUNGLE_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockNameEnum.JUNGLE_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.JUNGLE;
    }
}
