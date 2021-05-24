using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class SandBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2(0.125f, 0.875f), new Vector2(0.1875f, 0.875f),
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f)
            },
            /*SIDE*/
            {
                new Vector2(0.125f, 0.875f), new Vector2(0.1875f, 0.875f),
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.125f, 0.875f), new Vector2(0.1875f, 0.875f),
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f)
            }
        };

        public SandBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.SAND, pos, p, o)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
