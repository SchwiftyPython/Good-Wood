using Assets.Scripts.World.Noise;
using UnityEngine;

namespace Assets.Scripts
{
    public class Utils 
    {
        public static Perlin HighNoise;
        public static Perlin LowNoise;
        public static Perlin BottomNoise;
        public static Perlin CaveNoise;
        public static ClampNoise HighClamp;
        public static ClampNoise LowClamp;
        public static ClampNoise BottomClamp;
        public static ModifyNoise FinalNoise;

        public static int MaxHeight = 150;
        private static float _smooth = 0.01f;
        private static int _octaves = 4;
        private static float _persistence = .5f;

        private const int GroundLevel = 50;

        public static void InitializeNoise(int seed)
        {
            HighNoise = new Perlin(seed);
            LowNoise = new Perlin(seed);
            BottomNoise = new Perlin(seed);
            CaveNoise = new Perlin(seed);

            CaveNoise.Octaves = 3;
            CaveNoise.Amplitude = 0.05;
            CaveNoise.Persistance = 2;
            CaveNoise.Frequency = 0.05;
            CaveNoise.Lacunarity = 2;

            HighNoise.Persistance = 1;
            HighNoise.Frequency = 0.013;
            HighNoise.Amplitude = 10;
            HighNoise.Octaves = 2;
            HighNoise.Lacunarity = 2;

            LowNoise.Persistance = 1;
            LowNoise.Frequency = 0.004;
            LowNoise.Amplitude = 35;
            LowNoise.Octaves = 2;
            LowNoise.Lacunarity = 2.5;

            BottomNoise.Persistance = 0.5;
            BottomNoise.Frequency = 0.013;
            BottomNoise.Amplitude = 5;
            BottomNoise.Octaves = 2;
            BottomNoise.Lacunarity = 1.5;

            HighClamp = new ClampNoise(HighNoise);
            HighClamp.MinValue = -30;
            HighClamp.MaxValue = 50;

            LowClamp = new ClampNoise(LowNoise);
            LowClamp.MinValue = -30;
            LowClamp.MaxValue = 30;

            BottomClamp = new ClampNoise(BottomNoise);
            BottomClamp.MinValue = -20;
            BottomClamp.MaxValue = 5;

            FinalNoise = new ModifyNoise(HighClamp, LowClamp, NoiseModifier.Add);
        }

        public static int GetHeight(int x, int z)
        {
            var value = FinalNoise.Value2D(x, z) + GroundLevel;
            var coords = new Vector2(x, z);
            double distance = World.World.IsSpawnCoordinate(x, z) ? Vector2.Distance(coords, new Vector2(0, 64)) : 1000;
            if (distance < 1000) // Avoids deep water within 1km sq of spawn
                value += (1 - distance / 1000f) * 18;
            if (value < 0)
                value = GroundLevel;
            if (value > World.World.columnHeight)
                value = World.World.columnHeight - 1;
            return (int)value;
        }

        public static int GenerateHeight(float x, float z)
        {
            float height = Map(0,MaxHeight, 0, 1, FBm(x*_smooth,z*_smooth,_octaves,_persistence));
            return (int) height;
        }

        public static float FBm3D(float x, float y, float z, float sm, int oct)
        {
            float xy = FBm(x*sm,y*sm,oct,0.5f);
            float yz = FBm(y*sm,z*sm,oct,0.5f);
            float xz = FBm(x*sm,z*sm,oct,0.5f);

            float yx = FBm(y*sm,x*sm,oct,0.5f);
            float zy = FBm(z*sm,y*sm,oct,0.5f);
            float zx = FBm(z*sm,x*sm,oct,0.5f);

            return (xy+yz+xz+yx+zy+zx)/6.0f;
        }

        private static float Map(float newmin, float newmax, float origmin, float origmax, float value)
        {
            return Mathf.Lerp (newmin, newmax, Mathf.InverseLerp (origmin, origmax, value));
        }

        private static float FBm(float x, float z, int oct, float pers)
        {
            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            float maxValue = 0;
            float offset = 32000f;
            for(int i = 0; i < oct ; i++) 
            {
                total += Mathf.PerlinNoise((x+offset) * frequency, (z+offset) * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= pers;
                frequency *= 2;
            }

            return total/maxValue;
        }
    }
}
