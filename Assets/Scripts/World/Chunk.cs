using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets.Scripts.Blocks;
using Assets.Scripts.World;
using Assets.Scripts.World.Biomes;
using Assets.Scripts.World.Blocks;
using Assets.Scripts.World.Decorators;
using UnityEngine;

[Serializable]
class BlockData
{
    public Block.BlockType[,,] matrix;

    public BlockData() { }

    public BlockData(Block[,,] b)
    {
        matrix = new Block.BlockType[World.chunkSize, World.chunkSize, World.chunkSize];
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    matrix[x, y, z] = b[x, y, z].bType;
                }
    }
}

public class Chunk
{

    public Material cubeMaterial;
    public Material fluidMaterial;
    public Block[,,] chunkData;
    public GameObject chunk;
    public GameObject fluid;
    public enum ChunkStatus { DRAW, DONE, KEEP };
    public ChunkStatus status;
    public ChunkMB mb;
    BlockData bd;
    public bool changed = false;
    bool treesCreated = false;

    public BiomeProvider Biome;
    public IList<IChunkDecorator> ChunkDecorators { get; private set; }

    string BuildChunkFileName(Vector3 v)
    {
        return Application.persistentDataPath + "/savedata/Chunk_" +
                                (int)v.x + "_" +
                                    (int)v.y + "_" +
                                        (int)v.z +
                                        "_" + World.chunkSize +
                                        "_" + World.radius +
                                        ".dat";
    }

    bool Load() //read data from file
    {
        string chunkFile = BuildChunkFileName(chunk.transform.position);
		if(File.Exists(chunkFile))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(chunkFile, FileMode.Open);
			bd = new BlockData();
			bd = (BlockData) bf.Deserialize(file);
			file.Close();
			//Debug.Log("Loading chunk from file: " + chunkFile);
			return true;
		}
        return false;
    }

    public void Save() //write data to file
    {
        string chunkFile = BuildChunkFileName(chunk.transform.position);
		
		if(!File.Exists(chunkFile))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(chunkFile));
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(chunkFile, FileMode.OpenOrCreate);
		bd = new BlockData(chunkData);
		bf.Serialize(file, bd);
		file.Close();
        //Debug.Log("Saving chunk from file: " + chunkFile);
    }

    public IEnumerator UpdateChunk()
    {
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    if (chunkData[x, y, z].bType == Block.BlockType.SAND)
                    {
                        mb.StartCoroutine(mb.Drop(chunkData[x, y, z],
                            Block.BlockType.SAND,
                            20));
                    }
                    else if (chunkData[x, y, z].bType == Block.BlockType.WATER)
                    {
                        mb.StartCoroutine(mb.Flow(chunkData[x, y, z],
                            chunkData[x, y, z].GetBlockMaxHealth(), 15));
                    }

                    yield return null;
                }
            }
        }
    }

    void BuildChunk()
    {
        bool dataFromFile = false;
        dataFromFile = Load();

        var seed = World.Seed;
        var biomeRepo = World.BiomeRepo;

        var cPosition = chunk.transform.position;
        var id = World.BMap.GenerateBiome(seed, biomeRepo, cPosition, World.IsSpawnCoordinate((int)cPosition.x, (int)cPosition.y));
        var biomeCell = new BiomeCell(id, cPosition);
        World.BMap.AddCell(biomeCell);

        var biomeId = GetBiome(cPosition);
        var biome = biomeRepo.GetBiome(biomeId);

        Biome = (BiomeProvider)biome;

        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    int worldX = (int)(x + cPosition.x);
                    int worldY = (int)(y + cPosition.y);
                    int worldZ = (int)(z + cPosition.z);

                    if (dataFromFile)
                    {
                        chunkData[x, y, z] = new Block(bd.matrix[x, y, z], pos,
                            chunk.gameObject, this);
                        continue;
                    }

                    var location = new Vector2(x, y);

                    

                    int surfaceHeight = Utils.GenerateHeight((seed % Utils.maxHeight) + worldX, (seed % Utils.maxHeight) + worldZ);

                    if (worldY == 0)
                    {
                        chunkData[x, y, z] = new BedrockBlock(pos, chunk.gameObject, this);
                    }
                    else if (worldY == surfaceHeight)
                    {
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.4f, 2) < 0.4f)
                        {
                            chunkData[x, y, z] = new WoodbaseBlock(pos, chunk.gameObject, this);
                        }
                        else
                        {
                            chunkData[x, y, z] = Block.GetBlock(biome.SurfaceBlock, pos, chunk.gameObject, this);
                        }
                    }
                    else if (worldY < surfaceHeight)
                    {
                        if (worldY >= surfaceHeight - biome.FillerDepth)
                        {
                            chunkData[x, y, z] = Block.GetBlock(biome.FillerBlock, pos, chunk.gameObject, this);
                        }
                        else
                        {
                            chunkData[x, y, z] = new StoneBlock(pos, chunk.gameObject, this);
                        }
                    }
                    /*else if (worldY < 65)
                    {
                        chunkData[x, y, z] = new WaterBlock(pos, fluid.gameObject, this);
                        mb.Flow(chunkData[x, y, z], chunkData[x, y, z].GetBlockMaxHealth(), 15);
                    }     */               
                    else
                    {
                        chunkData[x, y, z] = new AirBlock(pos, chunk.gameObject, this);
                    }

                    if (chunkData[x, y, z].bType != Block.BlockType.WATER && Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 3) < 0.42f)
                    {
                        chunkData[x, y, z] = new AirBlock(pos, chunk.gameObject, this);
                    }

                    status = ChunkStatus.DRAW;
                }
            }
        }
        
        foreach (var decorator in ChunkDecorators)
        {
            decorator.Decorate(this, biomeRepo);
        }
    }

    public void Redraw()
    {
        GameObject.DestroyImmediate(chunk.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(chunk.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(chunk.GetComponent<Collider>());
        GameObject.DestroyImmediate(fluid.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(fluid.GetComponent<UvScroller>());
        GameObject.DestroyImmediate(fluid.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(fluid.GetComponent<Collider>());
        DrawChunk();
    }

    public void DrawChunk()
    {
        if (!treesCreated)
        {
            for (int z = 0; z < World.chunkSize; z++)
                for (int y = 0; y < World.chunkSize; y++)
                    for (int x = 0; x < World.chunkSize; x++)
                    {
                        BuildTrees(chunkData[x, y, z], x, y, z);
                    }
            treesCreated = true;
        }
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
        CombineQuads(chunk.gameObject, cubeMaterial);
        MeshCollider collider = chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunk.transform.GetComponent<MeshFilter>().mesh;

        CombineQuads(fluid.gameObject, fluidMaterial);

        fluid.AddComponent<UvScroller>();

        status = ChunkStatus.DONE;
    }

    void BuildTrees(Block trunk, int x, int y, int z)
    {
        if (trunk.bType != Block.BlockType.WOODBASE)
        {
            return;
        }

        Block t = trunk.GetBlock(x, y + 1, z);
        if (t != null)
        {
            //chunkData[x, y + 1, z] = new WoodBlock(t.position, chunk.gameObject, this);

            t.SetType(Block.BlockType.WOOD);

            Block t1 = t.GetBlock(x, y + 2, z);
            if (t1 != null)
            {
                //chunkData[x, y + 2, z] = new WoodBlock(t.position, chunk.gameObject, this);

                t1.SetType(Block.BlockType.WOOD);

                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        for (int k = 3; k <= 4; k++)
                        {
                            Block t2 = trunk.GetBlock(x + i, y + k, z + j);

                            if (t2 != null)
                            {
                                //chunkData[x + i, y + k, z + j] = new LeavesBlock(t2.position, chunk.gameObject, this);

                                t2.SetType(Block.BlockType.LEAVES);

                                // if (t2.owner.chunk.name == "0_112_0")
                                // {
                                //     Debug.Log("Trunk At: " + trunk.owner.chunk.name);
                                //     Debug.Log("Current Block " + trunk.position);
                                //     Debug.Log("Trying for: " + (x + i) + " " + (y + k) + " " + (z + j));
                                // }
                            }
                            else
                            {
                                return;
                            }
                        }
                Block t3 = t1.GetBlock(x, y + 5, z);
                if (t3 != null)
                {
                    //chunkData[x, y + 5, z] = new LeavesBlock(t3.position, chunk.gameObject, this);

                    t3.SetType(Block.BlockType.LEAVES);
                }
            }
        }
    }

    public Chunk() { }
    // Use this for initialization
    public Chunk(Vector3 position, Material c, Material t)
    {

        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        fluid = new GameObject(World.BuildChunkName(position) + "_F");
        fluid.transform.position = position;

        mb = chunk.AddComponent<ChunkMB>();
        mb.SetOwner(this);
        cubeMaterial = c;
        fluidMaterial = t;

        ChunkDecorators = new List<IChunkDecorator> {new LiquidDecorator()};

        BuildChunk();
    }


    public void CombineQuads(GameObject o, Material m)
    {
        //1. Combine all children meshes
        MeshFilter[] meshFilters = o.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)o.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = o.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = m;

        //5. Delete all uncombined children
        foreach (Transform quad in o.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }

    }

    private Guid GetBiome(Vector2 location)
    {
        return World.BMap.GetBiomeId(location);
    }

}
