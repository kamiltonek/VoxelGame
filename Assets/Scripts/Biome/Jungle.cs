using Assets.Scripts;
using Assets.Scripts.Enums;

public class Jungle : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.JUNGLE_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.JUNGLE_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.JUNGLE;
    }
}
