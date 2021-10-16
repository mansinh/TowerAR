
using UnityEngine;

[RequireComponent(typeof(SpawnEnemy))]
public class EnemySource : Destroyable
{
    SpawnEnemy _spawnEnemy;
    protected override void Init()
    {
        base.Init();
        _spawnEnemy = GetComponent<SpawnEnemy>();
    }

    void Update() {
        _spawnEnemy.Activate(transform.position);
    }
}