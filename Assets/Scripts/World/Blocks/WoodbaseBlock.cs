using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class WoodbaseBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
                new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)
            },
            /*SIDE*/
            {
                new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
                new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
                new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)
            }
        };

        public WoodbaseBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.WOODBASE, pos, p, o)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
