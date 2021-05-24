using System;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts.World.Blocks
{
    public class Block
    {

        enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
        public enum BlockType
        {
            GRASS, DIRT, WATER, STONE, LEAVES, WOOD, WOODBASE, SAND, GOLD, BEDROCK, REDSTONE, DIAMOND, NOCRACK,
            CRACK1, CRACK2, CRACK3, CRACK4, AIR
        };
        //todo make a stationary water blocktype that doesn't get a uv scroller attached

        public enum CrackedBlockType
        {
            NOCRACK,
            CRACK1,
            CRACK2,
            CRACK3,
            CRACK4
        }

        public BlockType bType;
        public bool isSolid;
        public Chunk owner;
        GameObject parent;
        public Vector3 position;

        public CrackedBlockType health;
        public int currentHealth;
        int[] blockHealthMax = { 3, 3, 10, 4, 2, 4, 4, 2, 3, -1, 4, 4, 0, 0, 0, 0, 0, 0 };

        private Dictionary<BlockType, int> _backupUvIndexes = new Dictionary<BlockType, int>
        {
            {BlockType.GRASS, 0},
            {BlockType.DIRT, 2},
            {BlockType.WATER, 3},
            {BlockType.STONE, 4},
            {BlockType.LEAVES, 5},
            {BlockType.WOOD, 6},
            {BlockType.WOODBASE, 7},
            {BlockType.SAND, 8},
            {BlockType.GOLD, 9},
            {BlockType.BEDROCK, 10},
            {BlockType.REDSTONE, 11},
            {BlockType.DIAMOND, 12},
            {BlockType.NOCRACK, 13},
            {BlockType.CRACK1, 14},
            {BlockType.CRACK2, 15},
            {BlockType.CRACK3, 16},
            {BlockType.CRACK4, 17},
        };

        private Vector2[,] _backupUvs =
        {
            /*GRASS TOP*/
            {
                new Vector2(0.125f, 0.375f), new Vector2(0.1875f, 0.375f),
                new Vector2(0.125f, 0.4375f), new Vector2(0.1875f, 0.4375f)
            },
            /*GRASS SIDE*/
            {
                new Vector2(0.1875f, 0.9375f), new Vector2(0.25f, 0.9375f),
                new Vector2(0.1875f, 1.0f), new Vector2(0.25f, 1.0f)
            },
            /*DIRT*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            },
            /*WATER*/
            {
                new Vector2(0.875f, 0.125f), new Vector2(0.9375f, 0.125f),
                new Vector2(0.875f, 0.1875f), new Vector2(0.9375f, 0.1875f)
            },
            /*STONE*/
            {
                new Vector2(0, 0.875f), new Vector2(0.0625f, 0.875f),
                new Vector2(0, 0.9375f), new Vector2(0.0625f, 0.9375f)
            },
            /*LEAVES*/
            {
                new Vector2(0.0625f, 0.375f), new Vector2(0.125f, 0.375f),
                new Vector2(0.0625f, 0.4375f), new Vector2(0.125f, 0.4375f)
            },
            /*WOOD*/
            {
                new Vector2(0.375f, 0.625f), new Vector2(0.4375f, 0.625f),
                new Vector2(0.375f, 0.6875f), new Vector2(0.4375f, 0.6875f)
            },
            /*WOODBASE*/
            {
                new Vector2(0.375f, 0.625f), new Vector2(0.4375f, 0.625f),
                new Vector2(0.375f, 0.6875f), new Vector2(0.4375f, 0.6875f)
            },
            /*SAND*/
            {
                new Vector2(0.125f, 0.875f), new Vector2(0.1875f, 0.875f),
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f)
            },
            /*GOLD*/
            {
                new Vector2(0f, 0.8125f), new Vector2(0.0625f, 0.8125f),
                new Vector2(0f, 0.875f), new Vector2(0.0625f, 0.875f)
            },
            /*BEDROCK*/
            {
                new Vector2(0.3125f, 0.8125f), new Vector2(0.375f, 0.8125f),
                new Vector2(0.3125f, 0.875f), new Vector2(0.375f, 0.875f)
            },
            /*REDSTONE*/
            {
                new Vector2(0.1875f, 0.75f), new Vector2(0.25f, 0.75f),
                new Vector2(0.1875f, 0.8125f), new Vector2(0.25f, 0.8125f)
            },
            /*DIAMOND*/
            {
                new Vector2(0.125f, 0.75f), new Vector2(0.1875f, 0.75f),
                new Vector2(0.125f, 0.8125f), new Vector2(0.1875f, 0.8125f)
            },
            /*NOCRACK*/
            {
                new Vector2(0.6875f, 0f), new Vector2(0.75f, 0f),
                new Vector2(0.6875f, 0.0625f), new Vector2(0.75f, 0.0625f)
            },
            /*CRACK1*/
            {
                new Vector2(0f, 0f), new Vector2(0.0625f, 0f),
                new Vector2(0f, 0.0625f), new Vector2(0.0625f, 0.0625f)
            },
            /*CRACK2*/
            {
                new Vector2(0.0625f, 0f), new Vector2(0.125f, 0f),
                new Vector2(0.0625f, 0.0625f), new Vector2(0.125f, 0.0625f)
            },
            /*CRACK3*/
            {
                new Vector2(0.125f, 0f), new Vector2(0.1875f, 0f),
                new Vector2(0.125f, 0.0625f), new Vector2(0.1875f, 0.0625f)
            },
            /*CRACK4*/
            {
                new Vector2(0.1875f, 0f), new Vector2(0.25f, 0f),
                new Vector2(0.1875f, 0.0625f), new Vector2(0.25f, 0.0625f)
            }
        };

        public Vector2[,] blockUVs =
        {
            /*TOP*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            },
            /*SIDE*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            },
            /*BOTTOM*/
            {
                new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f),
                new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)
            }
        };

        public Block(BlockType b, Vector3 pos, GameObject p, Chunk o)
        {
            bType = b;
            owner = o;
            parent = p;
            position = pos;
            SetType(bType);
        }

        public void SetType(BlockType b)
        {
            bType = b;
            if (bType == BlockType.AIR || bType == BlockType.WATER)
            {
                isSolid = false;
            }
            else
            {
                isSolid = true;
            }

            if (bType == BlockType.WATER)
            {
                parent = owner.fluid.gameObject;

                blockUVs = WaterBlock.MyUVs;
            }
            else
            {
                parent = owner.chunk.gameObject;
            }

            if (bType == BlockType.WOOD)
            {
                blockUVs = WoodBlock.MyUVs;
            }
            
            if (bType == BlockType.LEAVES)
            {
                blockUVs = LeavesBlock.MyUVs;
            }

            health = CrackedBlockType.NOCRACK;
            currentHealth = blockHealthMax[(int)bType];
        }

        public void Reset()
        {
            health = CrackedBlockType.NOCRACK;
            currentHealth = blockHealthMax[(int)bType];
            owner.Redraw();
        }

        public bool BuildBlock(BlockType b)
        {
            if (b == BlockType.WATER)
            {
                owner.mb.StartCoroutine(owner.mb.Flow(this,
                    BlockType.WATER,
                    blockHealthMax[(int)BlockType.WATER], 15));
            }
            else if (b == BlockType.SAND)
            {
                owner.mb.StartCoroutine(owner.mb.Drop(this,
                    BlockType.SAND,
                    20));
            }
            else
            {
                SetType(b);
                owner.Redraw();
            }
            return true;
        }

        public bool HitBlock()
        {
            if (currentHealth == -1)
            {
                return false;
            }

            currentHealth--;
            health++;

            if (currentHealth == (blockHealthMax[(int)bType] - 1))
            {
                owner.mb.StartCoroutine(owner.mb.HealBlock(position));
            }

            if (currentHealth <= 0)
            {
                bType = BlockType.AIR;
                isSolid = false;
                health = CrackedBlockType.NOCRACK;
                owner.Redraw();
                owner.mb.StartCoroutine(owner.UpdateChunk());
                return true;
            }

            owner.Redraw();
            return false;
        }

        void CreateQuad(Cubeside side)
        {
            Mesh mesh = new Mesh();
            mesh.name = "ScriptedMesh" + side.ToString();

            Vector3[] vertices = new Vector3[4];
            Vector3[] normals = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            List<Vector2> suvs = new List<Vector2>();
            int[] triangles = new int[6];

            //all possible UVs
            Vector2 uv00;
            Vector2 uv10;
            Vector2 uv01;
            Vector2 uv11;

            if (blockUVs == null)
            {
                Debug.LogError($"blockUVs null for {bType}!");
                return;
            }
            
            if (side == Cubeside.TOP)
            {
                uv00 = blockUVs[0, 0];
                uv10 = blockUVs[0, 1];
                uv01 = blockUVs[0, 2];
                uv11 = blockUVs[0, 3];
            }
            else if (side == Cubeside.BOTTOM)
            {
                uv00 = blockUVs[2, 0];
                uv10 = blockUVs[2, 1];
                uv01 = blockUVs[2, 2];
                uv11 = blockUVs[2, 3];
            }
            else
            {
                uv00 = blockUVs[1, 0];
                uv10 = blockUVs[1, 1];
                uv01 = blockUVs[1, 2];
                uv11 = blockUVs[1, 3];
            }

            SetCracks(suvs);

            //all possible vertices 
            Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
            Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
            Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
            Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

            switch (side)
            {
                case Cubeside.BOTTOM:
                    vertices = new Vector3[] { p0, p1, p2, p3 };
                    normals = new Vector3[] {Vector3.down, Vector3.down,
                        Vector3.down, Vector3.down};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.TOP:
                    vertices = new Vector3[] { p7, p6, p5, p4 };
                    normals = new Vector3[] {Vector3.up, Vector3.up,
                        Vector3.up, Vector3.up};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.LEFT:
                    vertices = new Vector3[] { p7, p4, p0, p3 };
                    normals = new Vector3[] {Vector3.left, Vector3.left,
                        Vector3.left, Vector3.left};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.RIGHT:
                    vertices = new Vector3[] { p5, p6, p2, p1 };
                    normals = new Vector3[] {Vector3.right, Vector3.right,
                        Vector3.right, Vector3.right};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.FRONT:
                    vertices = new Vector3[] { p4, p5, p1, p0 };
                    normals = new Vector3[] {Vector3.forward, Vector3.forward,
                        Vector3.forward, Vector3.forward};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.BACK:
                    vertices = new Vector3[] { p6, p7, p3, p2 };
                    normals = new Vector3[] {Vector3.back, Vector3.back,
                        Vector3.back, Vector3.back};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.SetUVs(1, suvs);
            mesh.triangles = triangles;

            mesh.RecalculateBounds();

            GameObject quad = new GameObject("Quad");
            quad.transform.position = position;
            quad.transform.parent = parent.transform;

            MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
            meshFilter.mesh = mesh;

        }

        private void SetCracks(ICollection<Vector2> suvs)
        {
            suvs.Add(CrackBlock.MyUVs[(int)health, 3]);
            suvs.Add(CrackBlock.MyUVs[(int)health, 2]);
            suvs.Add(CrackBlock.MyUVs[(int)health, 0]);
            suvs.Add(CrackBlock.MyUVs[(int)health, 1]);
        }

        int ConvertBlockIndexToLocal(int i)
        {
            if (i <= -1)
                i = Scripts.World.World.chunkSize + i;
            else if (i >= Scripts.World.World.chunkSize)
                i = i - Scripts.World.World.chunkSize;
            return i;
        }

        public BlockType GetBlockType(int x, int y, int z)
        {
            Block b = GetBlock(x, y, z);
            if (b == null)
                return BlockType.AIR;
            else
                return b.bType;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Block[,,] chunks;

            if (x < 0 || x >= Scripts.World.World.chunkSize ||
                y < 0 || y >= Scripts.World.World.chunkSize ||
                z < 0 || z >= Scripts.World.World.chunkSize)
            {  //block in a neighbouring chunk

                int newX = x, newY = y, newZ = z;
                if (x < 0 || x >= Scripts.World.World.chunkSize)
                    newX = (x - (int)position.x) * Scripts.World.World.chunkSize;
                if (y < 0 || y >= Scripts.World.World.chunkSize)
                    newY = (y - (int)position.y) * Scripts.World.World.chunkSize;
                if (z < 0 || z >= Scripts.World.World.chunkSize)
                    newZ = (z - (int)position.z) * Scripts.World.World.chunkSize;

                Vector3 neighbourChunkPos = this.parent.transform.position +
                                            new Vector3(newX, newY, newZ);
                string nName = Scripts.World.World.BuildChunkName(neighbourChunkPos);

                x = ConvertBlockIndexToLocal(x);
                y = ConvertBlockIndexToLocal(y);
                z = ConvertBlockIndexToLocal(z);

                Chunk nChunk;
                if (Scripts.World.World.chunks.TryGetValue(nName, out nChunk))
                {
                    chunks = nChunk.chunkData;
                }
                else
                    return null;
            }  //block in this chunk
            else
                chunks = owner.chunkData;

            return chunks[x, y, z];
        }

        public static Block GetBlock(BlockType bType, Vector3 position, GameObject parent, Chunk chunk)
        {
            switch (bType)
            {
                case BlockType.GRASS:
                    return new GrassBlock(position, parent, chunk);
                case BlockType.DIRT:
                    return new DirtBlock(position, parent, chunk);
                case BlockType.WATER:
                    return new WaterBlock(position, parent, chunk);
                case BlockType.STONE:
                    return new StoneBlock(position, parent, chunk);
                case BlockType.LEAVES:
                    return new LeavesBlock(position, parent, chunk);
                case BlockType.WOOD:
                    return new WoodBlock(position, parent, chunk);
                case BlockType.WOODBASE:
                    return new WoodbaseBlock(position, parent, chunk);
                case BlockType.SAND:
                    return new SandBlock(position, parent, chunk);
                case BlockType.GOLD:
                    return new StoneBlock(position, parent, chunk);//todo
                case BlockType.BEDROCK:
                    return new BedrockBlock(position, parent, chunk);
                case BlockType.REDSTONE:
                    return new RedstoneBlock(position, parent, chunk);
                case BlockType.DIAMOND:
                    return new DiamondBlock(position, parent, chunk);
                case BlockType.NOCRACK:
                    return new CrackBlock(position, parent, chunk);
                case BlockType.CRACK1:
                    return new CrackBlock(position, parent, chunk);
                case BlockType.CRACK2:
                    return new CrackBlock(position, parent, chunk);
                case BlockType.CRACK3:
                    return new CrackBlock(position, parent, chunk);
                case BlockType.CRACK4:
                    return new CrackBlock(position, parent, chunk);
                case BlockType.AIR:
                    return new AirBlock(position, parent, chunk);
                default:
                    throw new ArgumentOutOfRangeException(nameof(bType), bType, null);
            }
        }

        public int GetBlockMaxHealth()
        {
            return blockHealthMax[(int) bType];
        }

        public bool HasSolidNeighbour(int x, int y, int z)
        {
            try
            {
                Block b = GetBlock(x, y, z);
                if (b != null)
                    return (b.isSolid || b.bType == bType);
            }
            catch (System.IndexOutOfRangeException) { }

            return false;
        }

        public void Draw()
        {
            if (bType == BlockType.AIR) return;
            //solid or same neighbour
            if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
                CreateQuad(Cubeside.FRONT);
            if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
                CreateQuad(Cubeside.BACK);
            if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z))
                CreateQuad(Cubeside.TOP);
            if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
                CreateQuad(Cubeside.BOTTOM);
            if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
                CreateQuad(Cubeside.LEFT);
            if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
                CreateQuad(Cubeside.RIGHT);
        }
    }
}
