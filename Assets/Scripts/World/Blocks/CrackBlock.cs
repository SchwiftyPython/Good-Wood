using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class CrackBlock : Block
    {
        public static readonly Vector2[,] MyUVs =
        {
            /*NOCRACK*/
            {
                new Vector2(0.6875f, 0f), new Vector2(0.75f, 0f),
                new Vector2(0.6875f, 0.0625f), new Vector2(0.75f, 0.0625f)
            },
            /*CRACK1*/
            {
                new Vector2(0f, 0f), new Vector2(0.0625f, 0f),
                new Vector2(0f, 0.0625f), new Vector2(0.0625f, 0.0625f)
            },
            /*CRACK2*/
            {
                new Vector2(0.0625f, 0f), new Vector2(0.125f, 0f),
                new Vector2(0.0625f, 0.0625f), new Vector2(0.125f, 0.0625f)
            },
            /*CRACK3*/
            {
                new Vector2(0.125f, 0f), new Vector2(0.1875f, 0f),
                new Vector2(0.125f, 0.0625f), new Vector2(0.1875f, 0.0625f)
            },
            /*CRACK4*/
            {
                new Vector2(0.1875f, 0f), new Vector2(0.25f, 0f),
                new Vector2(0.1875f, 0.0625f), new Vector2(0.25f, 0.0625f)
            }
        };

        public CrackBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.NOCRACK, pos, p, o)
        {
        }
    }
}
