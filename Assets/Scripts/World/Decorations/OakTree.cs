using Assets.Scripts.World.Blocks;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.World.Decorations
{
    public class OakTree : Decoration
    {
        const int LeafRadius = 2;

        public override bool ValidLocation(Vector3 location)
        {
            if (location.x - LeafRadius < 0
                || location.x + LeafRadius >= World.chunkSize
                || location.z - LeafRadius < 0
                || location.z + LeafRadius >= World.chunkSize)
            {
                return false;
            }

            return true;
        }

        public override bool GenerateAt(Chunk chunk, Vector3 location)
        {
            if (!ValidLocation(location))
            {
                return false;
            }

            var random = new Random(World.Seed);
            int height = random.Next(4, 5);
            GenerateColumn(chunk, location, height, Block.BlockType.WOOD);
            Vector3 LeafLocation = location + new Vector3(0, height, 0);
            GenerateVanillaLeaves(chunk, LeafLocation, LeafRadius, Block.BlockType.LEAVES);
            return true;
        }
    }
}
