using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemy : Action
{
    Pool[] _enemyPools;
    [SerializeField] Enemy[] _enemyPrefabs;
    [SerializeField] int _enemyCount;
    [SerializeField] int _enemyType = -1;

    public void SetEnemyType(int enemyType) {
        _enemyType = enemyType;
    }

    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        if (_enemyType >= 0 && _enemyType < _enemyPools.Length)
        {
            if (_enemyPools[_enemyType].Push() == null)
            {
                SpawnRandom();
            }
        }
        else
        {
            SpawnRandom();
        }

    }

    void SpawnRandom()
    {
        _enemyPools[(int)(Random.value * _enemyPools.Length)].Push();
    }

    private void Start()
    {
        _enemyPools = new Pool[_enemyPrefabs.Length];
        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            _enemyPools[i] = gameObject.AddComponent<Pool>();
            _enemyPools[i].SetPrefab(_enemyPrefabs[i].gameObject);
            _enemyPools[i].SetPoolSize(_enemyCount);
            _enemyPools[i].Init();
        }
    }

    public void Reset()
    {
        foreach (Pool enemyPool in _enemyPools)
        {           
            foreach (GameObject active in enemyPool.Active)
            {
                Enemy enemy = active.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (enemy.isActiveAndEnabled)
                    {
                        enemy.TriggerDeath();
                    }
                }
            }
        }
    }
}
