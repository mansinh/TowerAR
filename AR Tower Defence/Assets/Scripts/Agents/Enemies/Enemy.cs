
using UnityEngine;

public class Enemy : Agent
{
    protected override void Init()
    {
        Name = "Enemy";
        TargetName = "Player";
        if (FindObjectOfType<Player>())
        {
            DefaultTarget = FindObjectOfType<Player>().transform;
        }
        base.Init();
    }

  
    protected override void Death()
    {
        Points.instance.EnemyKilled(Name);
        base.Death();
        
    }
}
 

