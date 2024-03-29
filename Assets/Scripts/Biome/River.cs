﻿using Assets.Scripts;
using Assets.Scripts.Enums;

public class River : Biome
{
    public River(float waterDistance) : base(waterDistance)
    {
    }

    public override int minHeight { get { return 1; } }

    public override int maxHeight { get { return 4; } }

    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockNameEnum.WATER];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockNameEnum.SAND];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.RIVER;
    }
}
