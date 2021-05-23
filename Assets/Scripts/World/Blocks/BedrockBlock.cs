using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BedrockBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2( 0.3125f, 0.8125f ), new Vector2( 0.375f, 0.8125f),
                new Vector2( 0.3125f, 0.875f ),new Vector2( 0.375f, 0.875f )
            },
            /*SIDE*/
            {
                new Vector2( 0.3125f, 0.8125f ), new Vector2( 0.375f, 0.8125f),
                new Vector2( 0.3125f, 0.875f ),new Vector2( 0.375f, 0.875f )
            },
            /*BOTTOM*/
            {
                new Vector2( 0.3125f, 0.8125f ), new Vector2( 0.375f, 0.8125f),
                new Vector2( 0.3125f, 0.875f ),new Vector2( 0.375f, 0.875f )
            }
        };

        public BedrockBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.BEDROCK, pos, p, o)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
