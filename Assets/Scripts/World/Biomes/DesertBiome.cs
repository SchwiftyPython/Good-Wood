using System;
using Assets.Scripts.World.Blocks;

namespace Assets.Scripts.World.Biomes
{
    public class DesertBiome : BiomeProvider
    {
        public override Guid Id
        {
            get
            {
                return BuildBiomeId(BiomeType.Desert);
            }
        }

        public override double Temperature => 2.0f;

        public override double Rainfall => 0.0f;

        public override bool Spawn => false;

        public override TreeSpecies[] Trees => new TreeSpecies[0];

        public override PlantSpecies[] Plants
        {
            get { return new[] {PlantSpecies.Deadbush, PlantSpecies.Cactus}; }
        }

        public override Block.BlockType SurfaceBlock => Block.BlockType.SAND;

        public override Block.BlockType FillerBlock => Block.BlockType.SAND; //todo change to sandstone

        public override int SurfaceDepth => 4;
    }
}
