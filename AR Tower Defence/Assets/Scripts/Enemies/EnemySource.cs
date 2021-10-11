using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySource : Destroyable
{
    [SerializeField] GameObject EnemyPrefab;
    int poolSize = 5;
    float timeSinceLastSpawn;
    public float spawnTime = 5;
    public bool isSpawning = false;

    List<Enemy> enemyPool = new List<Enemy>();
    List<Enemy> activeEnemies = new List<Enemy>();

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < poolSize; i++) {
            Enemy enemy = Instantiate(EnemyPrefab,WorldRoot.instance.transform).GetComponent<Enemy>();
            enemy.transform.parent = transform.parent;
       
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
            enemy.Init(this);
        }

        timeSinceLastSpawn = 10*(Random.value);
        isSpawning = true;
    }
    void Update()
    {
        if (isSpawning)
        {
            if (timeSinceLastSpawn > spawnTime && enemyPool.Count > 0)
            {
                Enemy enemy = enemyPool[0];
                activeEnemies.Add(enemy);
                enemyPool.Remove(enemy);
                enemy.transform.position = transform.position;
                enemy.gameObject.SetActive(true);
                enemy.Spawn();
                print("EnemySpawned");
                timeSinceLastSpawn = 0;
            }
            timeSinceLastSpawn += Time.deltaTime;
        }
    }

    public void OnEnemyDeath(Enemy enemy) {
        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        enemyPool.Add(enemy);
    }
}
