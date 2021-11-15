using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Agent
{
    protected override void Init()
    {
        base.Init();
        Perception.CheckLine = false;
    }

    protected override void Act()
    {
        if (CurrentTarget)
        {
            if (CurrentTarget.GetComponent<Barracks>())
            {
               
            }
            else
            {
                _view.gameObject.SetActive(true);
                base.Act();
            }
        }
    }
    protected override void LookAround()
    {
        base.LookAround();
        if (CurrentTarget)
        {
            if (World.Instance.GetTile(transform.position).GetState() < 100)
            {
                SetTarget(DefaultTarget, 0.2f);
            }
        }
    }
}
