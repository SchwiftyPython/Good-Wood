using System;
using System.Collections.Generic;
using Assets.Scripts.World.Biomes;
using Assets.Scripts.World.Noise;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class BiomeMap : IBiomeMap
    {
        private int _seed;
        private const float Offset = 32000f;

        private const float TempFrequency = 0.015f;
        private const float RainFrequency = 0.03f;

        Perlin TempNoise, RainNoise;

        public IList<BiomeCell> BiomeCells { get; }

        public BiomeMap(int seed)
        {
            _seed = seed;
            BiomeCells = new List<BiomeCell>();
            TempNoise = new Perlin(seed);
            RainNoise = new Perlin(seed);
            BiomeCells = new List<BiomeCell>();
            TempNoise.Persistance = 1.45;
            TempNoise.Frequency = 0.015;
            TempNoise.Amplitude = 5;
            TempNoise.Octaves = 2;
            TempNoise.Lacunarity = 1.3;
            RainNoise.Frequency = 0.03;
            RainNoise.Octaves = 3;
            RainNoise.Amplitude = 5;
            RainNoise.Lacunarity = 1.7;
            TempNoise.Seed = seed;
            RainNoise.Seed = seed;
        }

        public void AddCell(BiomeCell cell)
        {
            BiomeCells.Add(cell);
        }

        public Guid GetBiomeId(Vector2 location)
        {
            var biomeId = ClosestCell(location) != null ? ClosestCell(location).BiomeId : BiomeProvider.BuildBiomeId(BiomeType.Plains);
            return biomeId;
        }

        public Guid GenerateBiome(int seed, IBiomeRepository biomes, Vector2 location, bool spawn)
        {
            var temperature = Math.Abs(TempNoise.Value2D(location.x, location.y));
            var rain = Math.Abs(RainNoise.Value2D(location.x, location.y));

            var id = biomes.GetBiome(temperature, rain, spawn).Id;
            return id;
        }

        public BiomeCell ClosestCell(Vector2 location)
        {
            BiomeCell cell = null;
            var distance = double.MaxValue;
            foreach (BiomeCell c in BiomeCells)
            {
                var measuredDistance = Distance(location, c.CellPoint);
                if (measuredDistance < distance)
                {
                    distance = measuredDistance;
                    cell = c;
                }
            }
            return cell;
        }

        public double ClosestCellPoint(Vector2 location)
        {
            var distance = double.MaxValue;
            foreach (BiomeCell c in BiomeCells)
            {
                var measuredDistance = Distance(location, c.CellPoint);
                if (measuredDistance < distance)
                {
                    distance = measuredDistance;
                }
            }
            return distance;
        }

        public double Distance(Vector2 a, Vector2 b)
        {
            Vector2 diff = a - b;
            return Math.Max(Math.Abs(diff.x), Math.Abs(diff.y));
        }
    }
}
