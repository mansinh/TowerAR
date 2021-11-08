using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Agent
{
    public Transform Barracks;

    protected override void Init()
    {
        base.Init();
        Perception.CheckLine = false;
    }

    
}
