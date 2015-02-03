using UnityEngine;
using System.Collections;

//Base Rendering class, used to generate terrain and then recalculate meshes.
public interface Render_Base{

    void Initialize();

    void Render(Map curWorld, Chunk chunk);


    Mesh CreateMesh(Mesh mesh);
}
