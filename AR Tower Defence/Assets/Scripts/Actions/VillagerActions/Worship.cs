using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worship : Action
{
    [SerializeField] float worshipPoint = 3;
    protected override void Act(Vector3 targetPosition)
    {
        Points.Instance.AddPoints(worshipPoint);
    }
}
