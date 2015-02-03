using UnityEngine;
using System.Collections;

public class TerrainGenerator
{    
    public static void Generate(Map curWorld,Chunk chunk)
    {
        Debug.Log("IS THIS RUNNING?");
        for (int x = 0; x < curWorld.ChunkX; x++)
        {
            for (int z = 0; z <curWorld.ChunkZ; z++)
            {

                float terrain = 0.01f;

                float waterLvl = curWorld.ChunkY / 2;
                int height = (int)(SimplexNoise.noise((chunk.WorldX + x) * terrain, (chunk.WorldZ + z) * terrain) * curWorld.ChunkY / 12) + (curWorld.ChunkY / 3);
                //Debug.Log("Height: " + height);
                for (int y = 0; y < height; y++){
                    chunk.SetBlock(new DesertSand(), x,y,z);
                    //Debug.Log("Why is this not working");
                }
                }
            }
        }
    }
