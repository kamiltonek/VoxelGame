using Assets.Scripts;
using Assets.Scripts.Enums;

public class Desert : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.DESERT_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.DESERT_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.DESERT;
    }
}
