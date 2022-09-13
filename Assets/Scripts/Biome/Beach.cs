using Assets.Scripts;
using Assets.Scripts.Enums;

public class Beach : Biome
{
    public Beach(float waterDistance) : base(waterDistance)
    {
    }

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
        return BiomeNameEnum.BEACH;
    }
}
