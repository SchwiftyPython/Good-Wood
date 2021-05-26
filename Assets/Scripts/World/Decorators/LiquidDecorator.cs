using System;
using Assets.Scripts.World.Biomes;
using Assets.Scripts.World.Blocks;
using Assets.Scripts.World.Noise;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.World.Decorators
{
    public class LiquidDecorator : IChunkDecorator
    {
        private const int WaterLevel = 45;

        public Perlin Noise { get; set; }

        public void Decorate(Chunk chunk, IBiomeRepository biomeRepo)
        {
            Noise = new Perlin(World.Seed);

            for (int z = 0; z < World.chunkSize; z++)
            {
                for (int y = 0; y < World.columnHeight; y++)
                {
                    for (int x = 0; x < World.chunkSize; x++)
                    {
                        // if (Noise.Value2D(x, z) <= 0.3) //todo need some way to stop water from being in all underground caves or get liquids to settle without freezing game
                        // {
                        //     continue;
                        // }

                        var biome = chunk.Biome;

                        var worldY = (int)(y + chunk.chunk.transform.position.y);

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

                                //chunk.mb.StartCoroutine(chunk.mb.Flow(block, block.blockHealthMax[(int) Block.BlockType.WATER], 15));

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
