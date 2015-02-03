using UnityEngine;
using System.Collections;

/// <summary>
/// This class will handle the create of the world/ map for the current level or game.
/// </summary>

public class Map : MonoBehaviour
{

    #region Variables
    //public
    public int radius;
    public Vector3 chunkSize; // Variable to control the size of the chunks
    public GameObject mainCamera;
    public Material[] materials; // Material to add to the terrain
    public Render_Base renderer = new March_Render(); //Instance of marching cubes render
    //public Render_Base renderer = new Terrain_Render();
    //private
    private Grid<Chunk> grid = new Grid<Chunk>();
    private Profiler profiler = new Profiler();
    private int levelY = 50;
    #endregion


    /// <summary>
    /// Function used for simple Diagnostics on the screen.
    /// </summary>
    void OnGui()
    {
        Debug.Log("GUI?");
        GUILayout.Label("Current Memory Usage: " + (System.GC.GetTotalMemory(false) / (1024 * 1024)) + "\n" +
            "Grid Size : x " + grid.GetMinX() + "/" + grid.GetMaxX() + "z" + grid.GetMinZ() + "/" + grid.GetMaxZ() + "\n" +
            "Current Z Level: " + levelY);

    }

    //Swapped the start function for an awake function to make sure the world is generated before anything else.
    void Awake()
    {
        Debug.Log("Begin");
        renderer.Initialize();
    }

    void Update()
    {
        
        Chunk nearestEmptyChunk = NearestEmptyChunk();
        if(nearestEmptyChunk != null)
        {
            Debug.Log("UPDATE MAP");
            CreateChunk(nearestEmptyChunk);
            nearestEmptyChunk.Obj.MakeDirty();
        }
    }

    private Chunk NearestEmptyChunk()
    {
        Vector3 centre = mainCamera.transform.position;
        Vector3? near = null;

        for(int x = (int) centre.x - radius; x < (int)centre.x + radius; x++)
        {
            for(int z= (int)centre.z - radius; z<(int)centre.z + radius; z++)
            {
                if (GetChunkWPos(x, z).generated)
                    continue;
                Vector3 current = new Vector3(x, 0, z);
                float dist = Vector3.Distance(centre, current);
                if (dist > radius * radius)
                {
                    continue;
                }
                if (!near.HasValue)
                    near = current;
                else
                {
                    float _dist = Vector3.Distance(centre, near.Value);
                    if (dist < _dist)
                        near = current;
                }             
            }
        }
        if (near.HasValue)
        {
            return GetChunkWPos((int)near.Value.x, (int)near.Value.z);
        }
        return null;

    }
    public Block_Base GetBlock (int x, int y, int z)
    {
        Chunk chunk = GetChunkWPos(x, z);
        if(chunk.generated)
        {
            return chunk.GetBlockWPos(x, y, z);
        }
        return null;        
    }

    public void SetBlock(Block_Base block, int x, int y, int z)
    {

    }
    public void DeleteBlock(int x, int y, int z)
    {

    }

    public Chunk GetChunkWPos(int x, int z)
    {
        if (x < 0)
            x = (0 - ((ChunkX - x - 1) / ChunkX));
        else
            x = x / ChunkX;
        if (z < 0)
            z = (0 - ((ChunkZ - z - 1) / ChunkZ));
        else
            z = z / ChunkZ;
        return GetChunk(x, z);
    }
    public Chunk GetChunk(int x, int z)
    {
        Chunk chunk = grid.SafeGet(x, z);
        if (chunk == null)
        {
            chunk = new Chunk(this, x, z);
            grid.AddOrReplace(chunk, x, z);
        }
        return chunk;
    }


    public void UpdateBlockWorldPos(int x, int y, int z)
    {
        if (GetBlock(x + 1, y, z) != null)
        {
            GetBlock(x + 1, y, z).Update(this, x, y, z);
        }
        if (GetBlock(x, y + 1, z) != null)
        {
            GetBlock(x, y + 1, z).Update(this, x, y, z);
        }
        if (GetBlock(x, y, z + 1) != null)
        {
            GetBlock(x, y, z + 1).Update(this, x, y, z);
        }
        if (GetBlock(x - 1, y, z) != null)
        {
            GetBlock(x - 1, y, z).Update(this, x, y, z);
        }
        if (GetBlock(x, y - 1, z) != null)
        {
            GetBlock(x, y - 1, z).Update(this, x, y, z);
        }
        if (GetBlock(x, y, z - 1) != null)
        {
            GetBlock(x, y, z - 1).Update(this, x, y, z);
        }
    }

    public void RefreshChunkWorldPos(int x, int z)
    {
        Chunk chunk = GetChunkWPos(x, z);
        
        if (chunk.WorldLocateX(x) == 0)
        {
            GetChunk(chunk.X - 1, chunk.Z).Obj.MakeDirty();
        }
        if (chunk.WorldLocateX(x) == ChunkX - 1)
        {
            GetChunk(chunk.X + 1, chunk.Z).Obj.MakeDirty();
        }
        if (chunk.WorldLocateZ(z) == 0)
        {
            GetChunk(chunk.X, chunk.Z - 1).Obj.MakeDirty();
        }
        if (chunk.WorldLocateZ(x) == ChunkZ - 1)
        {
            GetChunk(chunk.X, chunk.Z + 1).Obj.MakeDirty();
        }
    }


    private void CreateChunk(Chunk chunk)
    {
        Debug.Log("Is this checked?");
        //If chunk has already been generated do nothing
        if (chunk.generated)
            return;
        //If no chunk generated create one and set boolean to true.
        TerrainGenerator.Generate(this, chunk);
        chunk.generated = true;
    }

    #region Get/Set
    //Holds information about the chunkSize
    public int ChunkX
    {
        get { return Mathf.RoundToInt(chunkSize.x); }
    }
    public int ChunkY
    { get { return Mathf.RoundToInt(chunkSize.y); } }
    public int ChunkZ
    { get { return Mathf.RoundToInt(chunkSize.z); } }
    public int LevelY
    { get { return levelY; } }
    #endregion

}
