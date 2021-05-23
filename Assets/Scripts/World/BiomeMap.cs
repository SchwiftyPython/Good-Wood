using System;
using System.Collections.Generic;
using Assets.Scripts.World.Biomes;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class BiomeMap : IBiomeMap
    {
        private int _seed;
        private const float Offset = 32000f;

        private const float TempFrequency = 0.015f;
        private const float RainFrequency = 0.03f;

        public IList<BiomeCell> BiomeCells { get; }

        public BiomeMap(int seed)
        {
            _seed = seed;
            BiomeCells = new List<BiomeCell>();
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
            var temperature = Math.Abs(Mathf.PerlinNoise((location.x + seed + Offset) * TempFrequency, (location.y + seed + Offset) * TempFrequency));
            var rain = Math.Abs(Mathf.PerlinNoise((seed + Offset) * RainFrequency, (seed + Offset) * RainFrequency));

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
