using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class GrassBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
                new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )
            },
            /*SIDE*/
            {
                new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
                new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )
            },
            /*BOTTOM*/
            {
                new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )
            }
        };

        public GrassBlock(Vector3 position, GameObject parent, Chunk chunk) : base(BlockType.GRASS, position, parent,
            chunk)
        {
            //bType = BlockType.GRASS;
            //parent = p;
            //position = pos;
            //cubeMaterial = c;

            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
