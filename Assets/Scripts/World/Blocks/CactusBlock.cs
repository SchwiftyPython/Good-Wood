using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class CactusBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2(0.375f,0.6875f),  new Vector2(0.4375f,0.6875f),
                new Vector2(0.375f,.75f), new Vector2(0.4375f,.75f)
            },
            /*SIDE*/
            {
                new Vector2(0.375f,0.6875f),  new Vector2(0.4375f,0.6875f),
                new Vector2(0.375f,.75f), new Vector2(0.4375f,.75f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.375f,0.6875f),  new Vector2(0.4375f,0.6875f),
                new Vector2(0.375f,.75f), new Vector2(0.4375f,.75f)
            }
        };

        public CactusBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.CACTUS, pos, p, o)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
