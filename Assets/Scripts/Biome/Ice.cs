using Assets.Scripts;
using Assets.Scripts.Enums;

public class Ice : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.ICE_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.ICE_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.ICE;
    }
}
