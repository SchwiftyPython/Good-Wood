using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class AirBlock : Block
    {
        public AirBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.AIR, pos, p, o)
        {
        }
    }
}
