using Assets.Scripts;
using Assets.Scripts.Enums;

public class Beach : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.BEACH_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.BEACH_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.BEACH;
    }
}
