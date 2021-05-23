using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class DiamondBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f),
                new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )
            },
            /*SIDE*/
            {
                new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f),
                new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )
            },
            /*BOTTOM*/
            {
                new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f),
                new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )
            }
        };

        public DiamondBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.DIAMOND, pos, p, o)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
