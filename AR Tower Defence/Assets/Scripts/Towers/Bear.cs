using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made a bear class by extending it from agent so they would behave as mele units that's attack enemies
public class Bear : Agent
{
    protected override void Init()
    {
        Name = "Bear";
        TargetName = "Enemy";
        if (FindObjectOfType<Enemy>())
        {
            DefaultTarget = FindObjectOfType<Enemy>().transform;
        }
        base.Init();
    }


    protected override void Remove()
    {
        Points.Instance.EnemyKilled(Name);
        base.Remove();

    }
}
