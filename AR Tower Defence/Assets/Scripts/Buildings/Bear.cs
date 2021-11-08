using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        base.Remove();

    }
}
