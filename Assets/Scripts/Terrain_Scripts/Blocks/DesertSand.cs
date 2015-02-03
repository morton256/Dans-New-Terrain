using UnityEngine;
using System.Collections;

public class DesertSand : Block_Base{

    public DesertSand()
    {
        this.alpha = false;
        this.uvBase = new Vector2(0, Random.Range(0, 2));        
    }
}
