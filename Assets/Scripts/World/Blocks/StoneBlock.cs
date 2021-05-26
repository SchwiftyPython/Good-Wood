using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class StoneBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )
            },
            /*SIDE*/
            {
                new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )
            },
            /*BOTTOM*/
            {
                new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )
            }
        };

        public StoneBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.STONE, pos, p, o)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
