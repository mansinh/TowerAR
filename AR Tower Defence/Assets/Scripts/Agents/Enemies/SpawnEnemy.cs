using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemy : Action
{
    Pool enemyPool;
    [SerializeField] Enemy _enemyPrefab;
    [SerializeField] int _enemyCount;


    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        enemyPool.Push();
    }
  
    private void Start()
    {
        enemyPool = gameObject.AddComponent<Pool>();
        enemyPool.SetPrefab(_enemyPrefab.gameObject);
        enemyPool.SetPoolSize(_enemyCount);
        enemyPool.Init();
    }
}
