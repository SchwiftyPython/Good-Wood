using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class DirtBlock : Block
    {
        private readonly Vector2[,] _myUVs =
        {
            /*TOP*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            },
            /*SIDE*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            }
        };

        public DirtBlock(Vector3 position, GameObject parent, Chunk chunk) : base(BlockType.GRASS, position, parent,
            chunk)
        {
            isSolid = true;
            blockUVs = _myUVs;
        }
    }
}
