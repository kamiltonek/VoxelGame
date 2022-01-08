using Assets.Scripts;
using Assets.Scripts.Enums;

public class Savanna : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.SAVANNA_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.SAVANNA_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.SAVANNA;
    }
}
