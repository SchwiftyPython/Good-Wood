using System;
using System.Collections;
using Assets.Scripts.World.Biomes;
using Assets.Scripts.World.Blocks;
using Assets.Scripts.World.Decorations;
using Assets.Scripts.World.Noise;
using UnityEngine;

namespace Assets.Scripts.World.Decorators
{
    public class TreeDecorator : IChunkDecorator
    {
        public Perlin Noise { get; set; }
        public ClampNoise ChanceNoise { get; set; }

        public void Decorate(Chunk chunk, IBiomeRepository biomeRepo)
        {
            Noise = new Perlin(World.Seed);
            ChanceNoise = new ClampNoise(Noise) {MaxValue = 2};
            Vector2? lastTree = null;
            for (int x = 0; x < World.chunkSize; x++)
            {
                for (int z = 0; z < World.chunkSize; z++)
                {
                    //todo probably need to use the code from BuildTrees here to at least get the surface height

                    var biome = chunk.Biome;
                    var blockX = Utils.ChunkToBlockX(x, (int) chunk.chunk.transform.position.x);
                    var blockZ = Utils.ChunkToBlockZ(z, (int)chunk.chunk.transform.position.z);
                    var height = Utils.GetHeight(x, z);

                    if (lastTree != null && Vector2.Distance(lastTree.Value, new Vector2(x,z)) < biome.TreeDensity)
                    {
                        continue;
                    }

                    var noiseValue = Noise.Value2D(blockX, blockZ);

                    if (noiseValue > 0.3)
                    {
                        if (height > World.columnHeight)
                        {
                            height = World.columnHeight - 1;
                        }

                        var location = new Vector3(x, height, z);

                        Block.BlockType bType;
                        try
                        {
                            bType = chunk.chunkData[(int)location.x, (int)location.y, (int)location.z].bType;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                        if (bType == Block.BlockType.DIRT || bType == Block.BlockType.GRASS)
                        {
                            var oakNoise = ChanceNoise.Value2D(blockX * 0.6, blockZ * 0.6);
                            var birchNoise = ChanceNoise.Value2D(blockX * 0.2, blockZ * 0.2);
                            var spruceNoise = ChanceNoise.Value2D(blockX * 0.35, blockZ * 0.35);

                            Debug.Log($"oakNoise: {oakNoise}");

                            var baseCoordinates = location + Vector3.up;
                            if (((IList) biome.Trees).Contains(TreeSpecies.Oak) && oakNoise > 1.01 && oakNoise < 1.25)
                            {
                                var oak = new OakTree().GenerateAt(chunk, baseCoordinates);
                                if (oak)
                                {
                                    lastTree = new Vector2(x, z);
                                    continue;
                                }
                            }
                            /*if (biome.Trees.Contains(TreeSpecies.Birch) && birchNoise > 0.3 && birchNoise < 0.95)
                            {
                                var birch = new BirchTree().GenerateAt(chunk, baseCoordinates);
                                if (birch)
                                {
                                    lastTree = new Vector2(x, z);
                                    continue;
                                }
                            }
                            if (biome.Trees.Contains(TreeSpecies.Spruce) && spruceNoise < 0.75)
                            {
                                var random = new Random(world.Seed);
                                var type = random.Next(1, 2);
                                var generated = false;
                                if (type.Equals(1))
                                    generated = new PineTree().GenerateAt(chunk, baseCoordinates);
                                else
                                    generated = new ConiferTree().GenerateAt(chunk, baseCoordinates);

                                if (generated)
                                {
                                    lastTree = new Vector2(x, z);
                                    continue;
                                }
                            }*/
                        }
                    }
                }
            }
        }
    }
}
