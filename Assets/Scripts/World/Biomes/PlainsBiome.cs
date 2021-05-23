using System;

namespace Assets.Scripts.World.Biomes
{
    public class PlainsBiome : BiomeProvider
    {
        public override Guid Id 
        {
            get
            {
                return BuildBiomeId(BiomeType.Plains);
            }
        }

        public override double Temperature 
        {
            get { return 0.8f; }
        }

        public override double Rainfall 
        {
            get { return 0.4f; }
        }

        public override TreeSpecies[] Trees 
        {
            get 
            {
                return new[] { TreeSpecies.Oak };
            }
        }
    }
}
