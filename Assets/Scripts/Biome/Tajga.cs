using Assets.Scripts;
using Assets.Scripts.Enums;

public class Tajga : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.TAJGA_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.TAJGA_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.TAJGA;
    }
}
