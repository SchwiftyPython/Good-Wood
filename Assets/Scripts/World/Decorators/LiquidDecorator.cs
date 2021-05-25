using System;
using Assets.Scripts.Blocks;
using Assets.Scripts.World.Biomes;
using Assets.Scripts.World.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.World.Decorators
{
    public class LiquidDecorator : IChunkDecorator
    {
        private const int WaterLevel = 45;

        public void Decorate(Chunk chunk, IBiomeRepository biomeRepo)
        {
            for (int z = 0; z < World.chunkSize; z++)
            {
                for (int y = 0; y < World.chunkSize; y++)
                {
                    for (int x = 0; x < World.chunkSize; x++)
                    {
                        var biome = chunk.Biome;

                        int worldY = (int)(y + chunk.chunk.transform.position.y);

                        for (int i = worldY; i < WaterLevel; i++)
                        {
                            Block block;
                            try
                            {
                                block = chunk.chunkData[x, (int)(i - chunk.chunk.transform.position.y), z];
                            }
                            catch (Exception e)
                            {
                                //gone into a neighboring chunk

                                //Debug.LogError($"Index out of bounds: x:{x}, y:{i - chunk.chunk.transform.position.y}, z:{z}");

                                continue;
                            }

                            if (block == null)
                            {
                                continue;
                            }

                            if (block.bType == Block.BlockType.AIR)
                            {
                                block.SetType(biome.WaterBlock);

                                if (i - chunk.chunk.transform.position.y < 1)
                                {
                                    continue;
                                }

                                var below = chunk.chunkData[x, (int) (i - chunk.chunk.transform.position.y - 1), z];

                                if (below.bType != Block.BlockType.AIR && below.bType != Block.BlockType.WATER)
                                {
                                    Random.InitState(World.Seed);

                                    var roll = Random.Range(1, 101);

                                    var cPosition = chunk.chunk.transform.position;
                                    if (roll < 40)
                                    {
                                        
                                        chunk.chunkData[x, (int)(i - cPosition.y - 1), z] = new DirtBlock(new Vector3(x, (int)(i - cPosition.y - 1), z),
                                            chunk.chunk.gameObject, chunk); //todo change to clay block
                                    }
                                    else
                                    {
                                        chunk.chunkData[x, (int)(i - cPosition.y - 1), z] = new SandBlock(new Vector3(x, (int)(i - cPosition.y - 1), z),
                                            chunk.chunk.gameObject, chunk);
                                    }
                                }
                            }
                        }
                        //todo lava
                    }
                }
            }
        }
    }
}
