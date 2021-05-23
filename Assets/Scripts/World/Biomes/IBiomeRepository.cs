using System;

namespace Assets.Scripts.World.Biomes
{
    public interface IBiomeRepository
    {
        IBiomeProvider GetBiome(Guid id);
        IBiomeProvider GetBiome(double temperature, double rainfall, bool spawn);
        void RegisterBiomeProvider(IBiomeProvider provider);
    }
}
