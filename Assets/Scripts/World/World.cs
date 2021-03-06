using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.World.Biomes;
using Assets.Scripts.World.Blocks;
using Realtime.Messaging.Internal;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.World
{
    public class World : MonoBehaviour {

        public GameObject player;
        public Material textureAtlas;
        public Material fluidTexture;
        public string SeedString;

        public static int Seed;
        public static int columnHeight = 32;
        public static int chunkSize = 16;
        public static int worldSize = 1;
        public static int radius = 3;
        public static uint maxCoroutines = 90000;
        public static ConcurrentDictionary<string, Chunk> chunks;
        public static List<string> toRemove = new List<string>();

        //USED FOR PROGRESS SLIDER
        public static bool firstbuild = true;
        public Slider buildProgress;
        public int chunkCount = 0;
        int totalChunkCount = 62; //work this out by running and looking at final chunk count.
        public Camera buildCam;
        public Button playButton;

        float startTime;

        public static CoroutineQueue queue;

        public Vector3 lastbuildPos;

        public static BiomeMap BMap;
        public static BiomeRepository BiomeRepo = new BiomeRepository();

        public static string BuildChunkName(Vector3 v)
        {
            return (int)v.x + "_" + 
                   (int)v.y + "_" + 
                   (int)v.z;
        }

        public static string BuildColumnName(Vector3 v)
        {
            return (int)v.x + "_" + (int)v.z;
        }

        public static Block GetWorldBlock(Vector3 pos)
        {
            int cx, cy, cz;
		
            if(pos.x < 0)
                cx = (int) (Mathf.Round(pos.x-chunkSize)/(float)chunkSize) * chunkSize;
            else
                cx = (int) (Mathf.Round(pos.x)/(float)chunkSize) * chunkSize;
		
            if(pos.y < 0)
                cy = (int) (Mathf.Round(pos.y-columnHeight)/(float)columnHeight) * columnHeight;
            else
                cy = (int) (Mathf.Round(pos.y)/(float)columnHeight) * columnHeight;
		

            if(pos.z < 0)
                cz = (int) (Mathf.Round(pos.z-chunkSize)/(float)chunkSize) * chunkSize;
            else
                cz = (int) (Mathf.Round(pos.z)/(float)chunkSize) * chunkSize;
	
            int blx = (int) Mathf.Abs((float)Math.Round(pos.x) - cx);
            int bly = (int) Mathf.Abs((float)Math.Round(pos.y) - cy);
            int blz = (int) Mathf.Abs((float)Math.Round(pos.z) - cz);

            string cn = BuildChunkName(new Vector3(cx,cy,cz));
            Chunk c;
            if(chunks.TryGetValue(cn, out c))
            {
                return c.chunkData[blx,bly,blz];
            }
            else
                return null;
        }

        void BuildChunkAt(int x, int y, int z)
        {
            Vector3 chunkPosition = new Vector3(x*chunkSize, 
                y* columnHeight, 
                z*chunkSize);
					
            string n = BuildChunkName(chunkPosition);
            Chunk c;

            if(!chunks.TryGetValue(n, out c))
            {
                c = new Chunk(chunkPosition, textureAtlas, fluidTexture);
                c.chunk.transform.parent = this.transform;
                c.fluid.transform.parent = this.transform;
                chunks.TryAdd(c.chunk.name, c);
                chunkCount ++;
                buildProgress.value = chunkCount/(float)totalChunkCount * 100;
                if(chunkCount == totalChunkCount)
                { 
                    firstbuild = false;
                    buildCam.gameObject.SetActive(false);
                    buildProgress.gameObject.SetActive(false);
                    playButton.gameObject.SetActive(false);
                }
            }
        }

        IEnumerator BuildRecursiveWorld(int x, int y, int z, int startrad, int rad)
        {
            int nextrad = rad-1;
            if(rad <= 0 || y < 0 || y > columnHeight) yield break;
            //build chunk front
            BuildChunkAt(x,y,z+1);
            queue.Run(BuildRecursiveWorld(x,y,z+1,rad,nextrad));
            yield return null;

            //build chunk back
            BuildChunkAt(x,y,z-1);
            queue.Run(BuildRecursiveWorld(x,y,z-1,rad,nextrad));
            yield return null;
		
            //build chunk left
            BuildChunkAt(x-1,y,z);
            queue.Run(BuildRecursiveWorld(x-1,y,z,rad,nextrad));
            yield return null;

            //build chunk right
            BuildChunkAt(x+1,y,z);
            queue.Run(BuildRecursiveWorld(x+1,y,z,rad,nextrad));
            yield return null;
		
            //build chunk up
            BuildChunkAt(x,y+1,z);
            queue.Run(BuildRecursiveWorld(x,y+1,z,rad,nextrad));
            yield return null;
		
            //build chunk down
            BuildChunkAt(x,y-1,z);
            queue.Run(BuildRecursiveWorld(x,y-1,z,rad,nextrad));
            yield return null;

            queue.Run(DrawChunks());

        }

        IEnumerator DrawChunks()
        {
            toRemove.Clear();
            foreach(KeyValuePair<string, Chunk> c in chunks)
            {
                if(c.Value.status == Chunk.ChunkStatus.DRAW) 
                {
                    c.Value.DrawChunk();
                }
                if(c.Value.chunk && Vector3.Distance(player.transform.position,
                    c.Value.chunk.transform.position) > radius*chunkSize)
                    toRemove.Add(c.Key);

                yield return null;
            }
        }

        IEnumerator RemoveOldChunks()
        {
            for(int i = 0; i < toRemove.Count; i++)
            {
                string n = toRemove[i];
                Chunk c;
                if(chunks.TryGetValue(n, out c))
                {
                    Destroy(c.chunk);
                    c.Save();
                    chunks.TryRemove(n, out c);
                    yield return null;
                }
            }
        }

        public void BuildNearPlayer()
        {
            StopCoroutine("BuildRecursiveWorld");
            queue.Run(BuildRecursiveWorld((int)(player.transform.position.x/chunkSize),
                (int)(player.transform.position.y/ columnHeight),
                (int)(player.transform.position.z/chunkSize),radius,radius));
        }

        public static bool IsSpawnCoordinate(int x, int y)
        {
            return x == 0 && y == 64;
        }

        // Use this for initialization
        void Start () 
        {
            Seed = ConvertSeed(SeedString);

            Utils.InitializeNoise(Seed);

            Vector3 ppos = player.transform.position;
            player.transform.position = new Vector3(ppos.x,
                Utils.GetHeight((int) ppos.x,(int) ppos.z) + 1,
                ppos.z);
            lastbuildPos = player.transform.position;
            player.SetActive(false);

            firstbuild = true;
            chunks = new ConcurrentDictionary<string, Chunk>();
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;	

            queue = new CoroutineQueue(maxCoroutines, StartCoroutine);
            startTime = Time.time;
            Debug.Log("Start Build");

            //build starting chunk
            /*BuildChunkAt((int)(player.transform.position.x/chunkSize),
											(int)(player.transform.position.y/chunkSize),
											(int)(player.transform.position.z/chunkSize));
		//draw it
		queue.Run(DrawChunks());

		//create a bigger world
		queue.Run(BuildRecursiveWorld((int)(player.transform.position.x/chunkSize),
											(int)(player.transform.position.y/chunkSize),
											(int)(player.transform.position.z/chunkSize),radius,radius));*/
        }

        public void PlayButtonPressed()
        {
            // Seed = ConvertSeed(SeedString);
            //
            // Utils.InitializeNoise(Seed);

            BMap = new BiomeMap(Seed);

            //build starting chunk
            BuildChunkAt((int)(player.transform.position.x/chunkSize),
                (int)(player.transform.position.y/ columnHeight),
                (int)(player.transform.position.z/chunkSize));
            //draw it
            queue.Run(DrawChunks());

            //create a bigger world
            queue.Run(BuildRecursiveWorld((int)(player.transform.position.x/chunkSize),
                (int)(player.transform.position.y/ columnHeight),
                (int)(player.transform.position.z/chunkSize),radius,radius));
        }

        private int ConvertSeed(string seedString)
        {
            var unicode = seedString.Select(t => $"U+{Convert.ToUInt16(t):X4} ").ToList();

            var newSeed = 0;
            var index = 0;
            foreach (var uni in unicode)
            {
                if (index >= uni.Length)
                {
                    index = 0;
                }

                var number = Convert.ToInt32(char.GetNumericValue(uni, index));

                number = (int) (Math.Pow(number, 2) + 1 % Int32.MaxValue);

                newSeed += number;

                index++;
            }

            Debug.Log($"String: {seedString} Int: {newSeed}");

            return newSeed;
        }
	
        // Update is called once per frame
        void Update () {

            if(firstbuild) return;
            Vector3 movement = lastbuildPos - player.transform.position;

            if(movement.magnitude > chunkSize )
            {
                lastbuildPos = player.transform.position;
                BuildNearPlayer();
            }

            if (!player.activeSelf)
            {
                player.SetActive(true);
                Debug.Log("Built in " + (Time.time - startTime));
                firstbuild = false;
            }

            queue.Run(DrawChunks());
            queue.Run(RemoveOldChunks());
        }
    }
}
