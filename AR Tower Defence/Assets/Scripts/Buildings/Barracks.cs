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
}