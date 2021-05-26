using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class WoodBlock : Block
    {
        public static readonly Vector2[,] MyUVs =
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

        public WoodBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.WOOD, pos, p, o)
        {
        }
    }
}
