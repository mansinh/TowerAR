using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnBearBear))]
public class Barracks : Tower
{

    void Update()
    {
        if (IsBuilt)
        {
            _attack.Activate(transform.position);
        }
    }

    public override string GetGameInfo(bool showState)
    {
        return "BARRACKS: Spawns up to 3 soldiers that attack enemies on sight. " + base.GetGameInfo(showState);
    }
}