
using UnityEngine;

public class Enemy : Agent
{
    protected override void Init()
    {
        Name = "Enemy";
        TargetName = "Player";
        if (FindObjectOfType<Shrine>())
        {
            DefaultTarget = FindObjectOfType<Shrine>().transform;
        }
        base.Init();
    }

  
    protected override void Remove()
    {
        //Points.Instance.EnemyKilled(Name);
        base.Remove();
        
    }
}
 

