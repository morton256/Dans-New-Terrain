using UnityEngine;
using System.Collections;

public class Water : Block_Base{

    public Water()
    {
        this.alpha = true;
        this.uvBase = new Vector2(0, Random.Range(6, 8));
    }

    protected override void OnUpdate(Map world, int x, int y, int z)
    {
        if (world.GetBlock(x, y, z) == null)
        {
            world.SetBlock(new Water(),x,y,z);
        }
    }
}
