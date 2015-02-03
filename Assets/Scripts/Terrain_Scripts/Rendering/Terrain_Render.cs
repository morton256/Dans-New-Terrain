using UnityEngine;
using System.Collections;
using System.Collections.Generic; //For Lists;

public class Terrain_Render : Render_Base
{
    #region Variables
    private Map curWorld;
    private Chunk chunk;
    private List<Vector3> verts;
    private List<Vector2> uvs;
    private List<int>[] tris;
    private static int subMeshes = 2;
    #endregion

    //Initialization
    public void Initialize()
    {
        this.uvs = new List<Vector2>();
        this.verts = new List<Vector3>();
        this.tris = new List<int>[subMeshes];
        for (int i = 0; i < subMeshes; i++)
        {
            tris[i] = new List<int>();
        }
        Debug.Log("IS this running?");
    }

    //Creates the blocks and adds them to chunk mesh
    public void Render(Map curWorld, Chunk chunk)
    {
        this.curWorld = curWorld;
        this.chunk = chunk;

        verts.Clear();
        uvs.Clear();
        for (int i = 0; i < subMeshes; i++)
            tris[i].Clear();

        Block_Base block;
        Vector3 start;

        for (int x = 0; x < curWorld.ChunkX; x++)
        {
            for (int y = 1; y < curWorld.LevelY; y++)
            {
                for (int z = 0; z < curWorld.ChunkZ; z++)
                {
                    start = new Vector3(x, y, z);
                    block = chunk.GetBlock(x, y, z);
                    if (block == null)
                        continue;

                    if (IsFaceVis(x - 1, y, z, block))
                        CreateFace(start + Vector3.back, start, start + Vector3.back + Vector3.up, start + Vector3.up, block);
                    if (IsFaceVis(x + 1, y, z, block))
                        CreateFace(start + Vector3.right, start + Vector3.right + Vector3.back, start + Vector3.right + Vector3.up, start +
                            Vector3.right + Vector3.back + Vector3.up, block);
                    if (IsFaceVis(x, y - 1, z, block))
                        CreateFace(start, start + Vector3.back, start + Vector3.right, start + Vector3.back + Vector3.right, block);
                    if (IsFaceVis(x, y + 1, z, block))
                        CreateFace(start + Vector3.up + Vector3.back, start + Vector3.up, start + Vector3.up + Vector3.back + Vector3.right, start + Vector3.up +
                            Vector3.right, block);
                    if (IsFaceVis(x, y, z - 1, block))
                        CreateFace(start + Vector3.back + Vector3.right, start + Vector3.back, start + Vector3.back + Vector3.right + Vector3.up, start + Vector3.back +
                            Vector3.up, block);
                    if (IsFaceVis(x, y, z + 1, block))
                        CreateFace(start, start + Vector3.right, start + Vector3.up, start + Vector3.right + Vector3.up, block);
                }
            }

        }
    }
    public Mesh CreateMesh(Mesh mesh)
    {
        if (verts.Count == 0)
        {
            GameObject.Destroy(mesh);
            return null;
        }
        if (mesh == null)
            mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.subMeshCount = tris.Length;
        for (int i = 0; i < tris.Length; i++)
        {
            mesh.SetTriangles(tris[i].ToArray(), i);
        }
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;

    }
    private void CreateFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Block_Base block)
    {
        int i = 0;
        if (block.Alpha)
        {
            i = 1;
        }

        int index = verts.Count;
        verts.Add(v1);
        verts.Add(v2);
        verts.Add(v3);
        if (v4 != Vector3.zero)
        {
            verts.Add(v4);
        }
        float tec = (1f / 8);
        Vector2 uvBase;

        if (block.top)
        {
            uvBase = new Vector2(1 * tec, Random.Range(0, 2) * tec);
        }
        else
        {
            uvBase = block.UV * tec;
        }
        uvs.Add(uvBase);
        uvs.Add(uvBase + new Vector2(tec, 0));

        if (v4 != Vector3.zero)
        {
            uvs.Add(uvBase + new Vector2(0, tec));
        }
        uvs.Add(uvBase + new Vector2(tec, tec));

        tris[i].Add(index + 0);
        tris[i].Add(index + 1);
        tris[i].Add(index + 2);

        if (v4 != Vector3.zero)
        {
            tris[i].Add(index + 3);
            tris[i].Add(index + 2);
            tris[i].Add(index + 1);
        }


    }
    private bool IsFaceVis(int x, int y, int z, Block_Base block)
    {
        bool showF = false;
        Block_Base neighbourBlock = GetBlock(x, y, z);

        if (neighbourBlock == null || (neighbourBlock.Alpha && !block.Alpha))
        {
            showF = true;
        }
        if (!showF && curWorld.LevelY == y)
        {
            block.top = true;
            return true;
        }
        else
        {
            block.top = false;
        }

        return showF;
    }
    private Block_Base GetBlock(int x, int y, int z)
    {
        if (y < 0 && y >= curWorld.ChunkY)
        {
            return null;
        }

        Block_Base block = null;

        if (x >= 0 && x < curWorld.ChunkX && z >= 0 && z < curWorld.ChunkZ)
        {
            block = chunk.GetBlock(x, y, z);
        }
        else
        {
            Chunk neighbourChunk = curWorld.GetChunkWPos(chunk.WorldX + x, chunk.WorldZ + z);
            if (neighbourChunk.generated)
            {
                block = neighbourChunk.GetBlockWPos(chunk.WorldX + x, y, chunk.WorldZ + z);
            }
        }

        return block;
    }
}
