using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : Biome
{
    protected override BlockType GenerateSurface()
    {
        return World.blockTypes[BlockName.SNOW];
    }

    protected override BlockType Generate1stLayer()
    {
        return World.blockTypes[BlockName.SNOW];
    }
}
