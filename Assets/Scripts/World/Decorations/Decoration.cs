using System;
using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.World.Decorations
{
    public abstract class Decoration : IDecoration
    {
        public virtual bool ValidLocation(Vector3 location) { return true; }

        public abstract bool GenerateAt(Chunk chunk, Vector3 location);

        public static bool IsCuboidWall(Vector2 location, Vector3 start, Vector3 size)
        {
            return location.x.Equals(start.x)
                || location.y.Equals(start.z)
                || location.x.Equals(start.x + (int)size.x - 1)
                || location.y.Equals(start.z + (int)size.z - 1);
        }

        public static bool IsCuboidCorner(Vector2 location, Vector3 start, Vector3 size)
        {
            return location.x.Equals(start.x) && location.y.Equals(start.z)
                || location.x.Equals(start.x) && location.y.Equals(start.z + (int)size.z - 1)
                || location.x.Equals(start.x + (int)size.x - 1) && location.y.Equals(start.z)
                || location.x.Equals(start.x + (int)size.x - 1) && location.y.Equals(start.z + (int)size.z - 1);
        }

        public static bool NeighborsBlock(Vector3 location, Block block, byte meta = 0x0)
        {
            var surrounding = new[] {
                location + Vector3.left,
                location + Vector3.right,
                location + Vector3.forward,
                location + Vector3.back,
            };
            foreach (var toCheck in surrounding)
            {
                if (toCheck.x < 0 || toCheck.x >= World.chunkSize || toCheck.z < 0 || toCheck.z >= World.chunkSize || toCheck.y < 0 || toCheck.y >= World.columnHeight)
                    return false;
                if (block.GetBlock((int) toCheck.x, (int) toCheck.y, (int) toCheck.z).bType == block.bType)
                {
                    return true;
                }
            }
            return false;
        }

        public static void GenerateColumn(Chunk chunk, Vector3 location, int height, Block.BlockType bType, byte meta = 0x0)
        {
            for (int offset = 0; offset < height; offset++)
            {
                var blockLocation = location + new Vector3(0, offset, 0);

                if (blockLocation.y >= World.columnHeight)
                {
                    return;
                }

                chunk.chunkData[(int) blockLocation.x, (int) blockLocation.y, (int) blockLocation.z] = Block.GetBlock(bType, blockLocation, chunk.chunk.gameObject, chunk);
            }
        }

        /*
         * Cuboid Modes
         * 0x0 - Solid cuboid of the specified block
         * 0x1 - Hollow cuboid of the specified block
         * 0x2 - Outlines the area of the cuboid using the specified block
         */
        public static void GenerateCuboid(Chunk chunk, Vector3 location, Vector3 size, Block.BlockType bType, byte meta = 0x0, byte mode = 0x0)
        {
            //If mode is 0x2 offset the size by 2 and change mode to 0x1
            if (mode.Equals(0x2))
            {
                size += new Vector3(2, 2, 2);
                mode = 0x1;
            }

            for (var w = location.x; w < location.x + size.x; w++)
            {
                for (var l = location.z; l < location.z + size.z; l++)
                {
                    for (var h = location.y; h < location.y + size.y; h++)
                    {
                        if (w < 0 || w >= World.chunkSize || l < 0 || l >= World.chunkSize || h < 0 || h >= World.columnHeight)
                        {
                            continue;
                        }

                        Vector3 blockLocation = new Vector3(w, h, l);
                        if (!h.Equals(location.y) && !h.Equals(location.y + (int)size.y - 1)
                            && !IsCuboidWall(new Vector2(w, l), location, size)
                            && !IsCuboidCorner(new Vector2(w, l), location, size))
                        {
                            continue;
                        }

                        chunk.chunkData[(int)blockLocation.x, (int)blockLocation.y, (int)blockLocation.z] = Block.GetBlock(bType, blockLocation, chunk.chunk.gameObject, chunk);
                    }
                }
            }
        }

        protected void GenerateVanillaLeaves(Chunk chunk, Vector3 location, int radius, Block.BlockType bType, byte meta = 0x0)
        {
            int radiusOffset = radius;
            for (int yOffset = -radius; yOffset <= radius; yOffset = (yOffset + 1))
            {
                var y = location.y + yOffset;
                if (y > World.columnHeight)
                {
                    continue;
                }

                GenerateVanillaCircle(chunk, new Vector3(location.x, y, location.z), radiusOffset, bType, meta);
                if (yOffset != -radius && yOffset % 2 == 0)
                {
                    radiusOffset--;
                }
            }
        }

        protected void GenerateVanillaCircle(Chunk chunk, Vector3 location, int radius, Block.BlockType bType, byte meta = 0x0, double corner = 0)
        {
            for (int i = -radius; i <= radius; i = (i + 1))
            {
                for (int j = -radius; j <= radius; j = (j + 1))
                {
                    int max = (int)Math.Sqrt((i * i) + (j * j));
                    if (max <= radius)
                    {
                        if (i.Equals(-radius) && j.Equals(-radius)
                            || i.Equals(-radius) && j.Equals(radius)
                            || i.Equals(radius) && j.Equals(-radius)
                            || i.Equals(radius) && j.Equals(radius))
                        {
                            if (corner + radius * 0.2 < 0.4 || corner + radius * 0.2 > 0.7 || corner.Equals(0))
                                continue;
                        }
                        var x = location.x + i;
                        var z = location.z + j;
                        var currentBlock = new Vector3(x, location.y, z);

                        Block.BlockType otherBType;
                        try
                        {
                            otherBType = chunk.chunkData[(int)location.x, (int)location.y, (int)location.z].bType;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                        if (otherBType == Block.BlockType.AIR)
                        {
                            chunk.chunkData[(int)currentBlock.x, (int)currentBlock.y, (int)currentBlock.z] = Block.GetBlock(bType, currentBlock, chunk.chunk.gameObject, chunk);
                        }
                    }
                }
            }
        }

        protected void GenerateCircle(Chunk chunk, Vector3 location, int radius, Block.BlockType bType)
        {
            for (int i = -radius; i <= radius; i = (i + 1))
            {
                for (int j = -radius; j <= radius; j = (j + 1))
                {
                    int max = (int)Math.Sqrt((i * i) + (j * j));
                    if (max <= radius)
                    {
                        var x = location.x + i;
                        var z = location.z + j;

                        if (x < 0 || x >= World.chunkSize || z < 0 || z >= World.chunkSize)
                            continue;

                        var currentBlock = new Vector3(x, location.y, z);
                        if (chunk.chunkData[(int)currentBlock.x, (int)currentBlock.y, (int)currentBlock.z].bType == Block.BlockType.AIR)
                        {
                            chunk.chunkData[(int)currentBlock.x, (int)currentBlock.y, (int)currentBlock.z] = Block.GetBlock(bType, currentBlock, chunk.chunk.gameObject, chunk);
                        }
                    }
                }
            }
        }

        protected static void GenerateSphere(Chunk chunk, Vector3 location, int radius, Block.BlockType bType)
        {
            for (int i = -radius; i <= radius; i = (i + 1))
            {
                for (int j = -radius; j <= radius; j = (j + 1))
                {
                    for (int k = -radius; k <= radius; k = (k + 1))
                    {
                        int max = (int)Math.Sqrt((i * i) + (j * j) + (k * k));
                        if (max <= radius)
                        {
                            var x = location.x + i;
                            var y = location.y + k;
                            var z = location.z + j;

                            if (x < 0 || x >= World.chunkSize || z < 0 || z >= World.chunkSize || y < 0 || y >= World.columnHeight)
                                continue;

                            var currentBlock = new Vector3(x, y, z);
                            if (chunk.chunkData[(int)currentBlock.x, (int)currentBlock.y, (int)currentBlock.z].bType == Block.BlockType.AIR)
                            {
                                chunk.chunkData[(int)currentBlock.x, (int)currentBlock.y, (int)currentBlock.z] = Block.GetBlock(bType, currentBlock, chunk.chunk.gameObject, chunk);
                            }
                        }
                    }
                }
            }
        }
    }
}
