
using UnityEngine;

public class Enemy : Agent
{
    protected override void Init()
    {
        Name = "Enemy";
        TargetName = "Village";
        if (FindObjectOfType<Shrine>())
        {
            DefaultTarget = FindObjectOfType<Shrine>().transform;
        }
        base.Init();
    }

  
    protected override void Remove()
    {
        
        base.Remove();
        
    }
}
 

