using UnityEngine;
using System.Collections;

public class Block_Base {

    protected bool alpha = false;
    protected Vector2 uvBase;
    public bool selected;
    public bool top;

    public bool Alpha { get { return alpha; } }

    public Vector2 UV { get { return uvBase; } }

    protected virtual void OnUpdate(Map world, int x, int y, int z)
    {
        // override to use
    }

    public void Update(Map world, int x, int y, int z)
    {
        OnUpdate(world, x, y, z);
    }
}
