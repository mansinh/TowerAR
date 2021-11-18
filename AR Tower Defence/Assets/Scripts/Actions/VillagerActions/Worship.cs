using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worship : Action
{
    [SerializeField] float worshipPoint = 3;
    Shrine shrine;
    protected override void Init()
    {
        base.Init();
        shrine = FindObjectOfType<Shrine>();
    }

    protected override void Act(Vector3 targetPosition)
    {
        if (shrine)
        {
            shrine.OnWorship();
        }
        Points.Instance.AddPoints(worshipPoint);
    }
}
