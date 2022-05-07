using Assets.Scripts;
using Assets.Scripts.Enums;

public class Beach : Biome
{
    public Beach(float waterDistance) : base(waterDistance)
    {
    }

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
        return BiomeNameEnum.BEACH;
    }
}
