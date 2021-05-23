using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Assets.Scripts.World.Blocks;
using UnityEngine;

public class AirBlock : Block
{
    public AirBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.AIR, pos, p, o)
    {
    }
}
