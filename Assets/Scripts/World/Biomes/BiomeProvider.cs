using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Blocks;
using Assets.Scripts.World.Blocks;
using UnityEngine;

namespace Assets.Scripts.World.Biomes
{
    public abstract class BiomeProvider : IBiomeProvider
    {
        public virtual bool Spawn 
        {
            get
            {
                return true;
            }
        }
        public abstract Guid Id { get; }
        public virtual int Elevation { get { return 0; } }

        /// <summary>
        /// The base temperature of the biome.
        /// </summary>
        public abstract double Temperature { get; }
        
        /// <summary>
        /// The base rainfall of the biome.
        /// </summary>
        public abstract double Rainfall { get; }

        /// <summary>
        /// The tree types generated in the biome.
        /// </summary>
        public virtual TreeSpecies[] Trees 
        {
            get 
            {
                return new[] { TreeSpecies.Oak };
            }
        }

        /// <summary>
        /// The plants generated in the biome.
        /// </summary>
        public virtual PlantSpecies[] Plants 
        {
            get
            {
                return new[] { PlantSpecies.Dandelion, PlantSpecies.Rose, PlantSpecies.TallGrass, PlantSpecies.Fern };
            }
        }

        /// <summary>
        /// The ores generated in the biome.
        /// </summary>
        public virtual OreTypes[] Ores 
        {
            get 
            {
                return new[] { OreTypes.Coal, OreTypes.Iron, OreTypes.Lapiz, OreTypes.Redstone, OreTypes.Gold, OreTypes.Diamond };
            }
        }

        /// <summary>
        /// The required distance between trees.
        /// </summary>
        public virtual double TreeDensity { get { return 4; } }

        /// <summary>
        /// The block used to fill water features such as lakes, rivers, etc.
        /// </summary>
        public virtual Block.BlockType WaterBlock
        {
            get { return Block.BlockType.WATER; } 
        }

        /// <summary>
        /// The main surface block used for the terrain of the biome.
        /// </summary>
        public virtual Block.BlockType SurfaceBlock { get { return Block.BlockType.GRASS; } }

        /// <summary>
        /// The main "filler" block found under the surface block in the terrain of the biome.
        /// </summary>
        public virtual Block.BlockType FillerBlock { get { return Block.BlockType.DIRT; } }

        /// <summary>
        /// The depth of the surface block layer
        /// </summary>
        public virtual int SurfaceDepth { get { return 1; } }

        /// <summary>
        /// The depth of the "filler" blocks  located below the surface block layer
        /// </summary>
        public virtual int FillerDepth { get { return 4; } }

        public static Guid BuildBiomeId(BiomeType bType)
        {
            List<byte> bytes = BitConverter.GetBytes((long) bType).Take(16).ToList();

            bytes.AddRange(BitConverter.GetBytes((long)bType).Take(16).ToList());

            //Debug.Log($"{bType} Biome Id: {new Guid(bytes.ToArray())}");

            return new Guid(bytes.ToArray());
        }
    }
}
