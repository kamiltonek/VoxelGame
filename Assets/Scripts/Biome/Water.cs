using Assets.Scripts;
using Assets.Scripts.Enums;

public class Water : Biome
{
    public Water(float waterDistance) : base(waterDistance)
    {
    }

    public override int minHeight { get { return 1; } }

    public override int maxHeight { get { return 4; } }
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockNameEnum.SAND];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockNameEnum.SAND];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.WATER;
    }
}
