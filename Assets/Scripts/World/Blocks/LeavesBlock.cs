using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class LeavesBlock : Block
    {
        public static readonly Vector2[,] MyUVs =
        {
            /*TOP*/
            {
                new Vector2(0.0625f,0.375f),  new Vector2(0.125f,0.375f),
                new Vector2(0.0625f,0.4375f), new Vector2(0.125f,0.4375f)
            },
            /*SIDE*/
            {
                new Vector2(0.0625f,0.375f),  new Vector2(0.125f,0.375f),
                new Vector2(0.0625f,0.4375f), new Vector2(0.125f,0.4375f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.0625f,0.375f),  new Vector2(0.125f,0.375f),
                new Vector2(0.0625f,0.4375f), new Vector2(0.125f,0.4375f)
            }
        };

        public LeavesBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.LEAVES, pos, p, o)
        {
        }
    }
}
