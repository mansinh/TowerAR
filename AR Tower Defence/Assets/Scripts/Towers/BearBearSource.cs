using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made source that'd be spawning bearbears
[RequireComponent(typeof(SpawnBearBear))]
public class BearBearSource : Destroyable
{
    SpawnBearBear _spawnBearBear;
    protected override void Init()
    {
        base.Init();
        _spawnBearBear = GetComponent<SpawnBearBear>();
    }

    void Update()
    {
        _spawnBearBear.Activate(transform.position);
    }
}