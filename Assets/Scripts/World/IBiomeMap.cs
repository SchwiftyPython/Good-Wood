using System;
using System.Collections.Generic;
using Assets.Scripts.World.Biomes;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class BiomeCell
    {
        public Guid BiomeId;
        public Vector2 CellPoint;

        public BiomeCell(Guid biomeId, Vector2 cellPoint)
        {
            BiomeId = biomeId;
            CellPoint = cellPoint;
        }
    }

    public interface IBiomeMap
    {
        IList<BiomeCell> BiomeCells { get; }
        void AddCell(BiomeCell cell);
        Guid GetBiomeId(Vector2 location);
        Guid GenerateBiome(int seed, IBiomeRepository biomes, Vector2 location, bool spawn);
        BiomeCell ClosestCell(Vector2 location);
        double ClosestCellPoint(Vector2 location);
    }
}
