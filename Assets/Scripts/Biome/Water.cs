using Assets.Scripts;
using Assets.Scripts.Enums;

public class Water : Biome
{
    public override int minHeight { get { return 1; } }

    public override int maxHeight { get { return 4; } }
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.SAND];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.SAND];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.WATER;
    }
}
