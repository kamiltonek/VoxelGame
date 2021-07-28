using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.SAND];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.SAND];
    }
}
