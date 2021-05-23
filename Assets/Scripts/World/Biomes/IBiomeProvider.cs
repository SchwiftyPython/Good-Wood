using System;
using Assets.Scripts.World.Blocks;

namespace Assets.Scripts.World.Biomes
{
    public interface IBiomeProvider
    {
        bool Spawn { get; }
        Guid Id { get; }
        int Elevation { get; }
        double Temperature { get; }
        double Rainfall { get; }
        TreeSpecies[] Trees { get; }
        PlantSpecies[] Plants { get; }
        OreTypes[] Ores { get; }
        double TreeDensity { get; }
        Block.BlockType WaterBlock { get; }
        Block.BlockType SurfaceBlock { get; }
        Block.BlockType FillerBlock { get; }
        int SurfaceDepth { get; }
        int FillerDepth { get; }
    }
}
