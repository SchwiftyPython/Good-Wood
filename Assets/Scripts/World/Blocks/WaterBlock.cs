using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class WaterBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2(0.875f,0.125f),  new Vector2(0.9375f,0.125f),
                new Vector2(0.875f,0.1875f), new Vector2(0.9375f,0.1875f)
            },
            /*SIDE*/
            {
                new Vector2(0.875f,0.125f),  new Vector2(0.9375f,0.125f),
                new Vector2(0.875f,0.1875f), new Vector2(0.9375f,0.1875f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.875f,0.125f),  new Vector2(0.9375f,0.125f),
                new Vector2(0.875f,0.1875f), new Vector2(0.9375f,0.1875f)
            }
        };

        public WaterBlock(Vector3 pos, GameObject p, Chunk o) : base(BlockType.WATER, pos, p, o)
        {
            blockUVs = _myUVs;
        }
    }
}
