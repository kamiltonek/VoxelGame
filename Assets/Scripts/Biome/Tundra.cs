﻿using Assets.Scripts;
using Assets.Scripts.Enums;

public class Tundra : Biome
{
    public Tundra(float waterDistance) : base(waterDistance)
    {
    }

    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.TUNDRA_BLOCK];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.TUNDRA_BLOCK];
    }

    public override BiomeNameEnum GetBiomeName()
    {
        return BiomeNameEnum.TUNDRA;
    }
}