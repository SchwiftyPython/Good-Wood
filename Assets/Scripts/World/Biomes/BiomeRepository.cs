using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.World.Biomes
{
    public class BiomeRepository : IBiomeRepository
    {
        private readonly Dictionary<Guid, IBiomeProvider> _biomeProviders = new Dictionary<Guid, IBiomeProvider>();

        public BiomeRepository()
        {
            DiscoverBiomes();
        }

        private void DiscoverBiomes()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(t => typeof(IBiomeProvider).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        var instance = (IBiomeProvider)Activator.CreateInstance(type);
                        RegisterBiomeProvider(instance);
                    }
                }
                catch
                {
                    // There are some bugs with loading mscorlib during a unit test like this
                }
            }
        }

        public IBiomeProvider GetBiome(Guid id)
        {
            return _biomeProviders[id];
        }

        public IBiomeProvider GetBiome(double temperature, double rainfall, bool spawn)
        {
            List<IBiomeProvider> temperatureResults = new List<IBiomeProvider>();
            foreach (var biome in _biomeProviders.Values)
            {
                if (biome != null && biome.Temperature.Equals(temperature))
                {
                    temperatureResults.Add(biome);
                }
            }

            if (temperatureResults.Count.Equals(0))
            {
                IBiomeProvider provider = null;
                float temperatureDifference = 100.0f;
                foreach (var biome in _biomeProviders.Values)
                {
                    if (biome != null)
                    {
                        var difference = Math.Abs(temperature - biome.Temperature);
                        if (provider == null || difference < temperatureDifference)
                        {
                            provider = biome;
                            temperatureDifference = (float)difference;
                        }
                    }
                }
                temperatureResults.Add(provider);
            }

            foreach (var biome in _biomeProviders.Values)
            {
                if (biome != null
                    && biome.Rainfall.Equals(rainfall)
                    && temperatureResults.Contains(biome)
                    && (!spawn || biome.Spawn))
                {
                    return biome;
                }
            }

            IBiomeProvider biomeProvider = null;
            float rainfallDifference = 100.0f;
            foreach (var biome in _biomeProviders.Values)
            {
                if (biome != null)
                {
                    var difference = Math.Abs(temperature - biome.Temperature);
                    if ((biomeProvider == null || difference < rainfallDifference)
                        && (!spawn || biome.Spawn))
                    {
                        biomeProvider = biome;
                        rainfallDifference = (float)difference;
                    }
                }
            }
            return biomeProvider ?? new PlainsBiome();
        }

        public void RegisterBiomeProvider(IBiomeProvider provider)
        {
            _biomeProviders[provider.Id] = provider;
        }
    }
}
