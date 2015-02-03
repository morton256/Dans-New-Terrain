using UnityEngine;
using System.Collections;

public class Chunk_Object : MonoBehaviour
{
    #region Variables
    private Map curWorld;
    private Chunk chunk;
    private MeshFilter mFilter;
    private MeshCollider mCol;

    private bool dirty = false;
    private float offset;
    private bool add = true;
    private int lastLvlY;
    #endregion

    //Create new chunk
    public static Chunk_Object Instance (Map curWorld, Chunk chunk)
    {
        Debug.Log("CHUNK OBJECT RUNNING");
        GameObject gObject = new GameObject("Chunk : (" + chunk.WorldX + "," + chunk.WorldZ + ")");
        gObject.transform.parent = curWorld.transform; //Parent it to the current world
        gObject.transform.position = new Vector3(chunk.WorldX, 0, chunk.WorldZ); //Position for the new chunk
        gObject.transform.rotation = Quaternion.identity;

        gObject.AddComponent<MeshRenderer>().sharedMaterials = curWorld.materials; //materials for the chunk
        gObject.renderer.castShadows = true;
        gObject.renderer.receiveShadows = true;

        Chunk_Object chunkObj = gObject.AddComponent<Chunk_Object>();
        //initializes the object;
        chunkObj.Initialize(curWorld, chunk, gObject.AddComponent<MeshFilter>(), gObject.AddComponent<MeshCollider>());
        return chunkObj;        
    }

    public void Initialize(Map curWorld, Chunk chunk, MeshFilter mFilter, MeshCollider mCol)
    {
        this.mFilter = mFilter;
        this.mCol = mCol;
        this.curWorld = curWorld;
        this.chunk = chunk;
        this.lastLvlY = curWorld.LevelY;
        Debug.Log("CHUNK OBJECT RUNNING");
    }

    public void Update()
    {
        if (chunk.NeighboursReady() &&
                    (this.dirty ||
                    (this.lastLvlY != curWorld.LevelY && renderer.isVisible)))
        {
            mFilter.sharedMesh = RMesh();
            mCol.sharedMesh = null;
            mCol.sharedMesh = mFilter.sharedMesh;
            this.lastLvlY = curWorld.LevelY;
            this.dirty = false;
        }
        
    }

    private Mesh RMesh()
    {
        Debug.Log("Render Mesh?");
        curWorld.renderer.Render(curWorld, chunk);
        return curWorld.renderer.CreateMesh(mFilter.sharedMesh);
    }

    public void MakeDirty()
    {
        this.dirty = true;
    }
}
