using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySource : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    float timeSinceLastSpawn;
    [SerializeField] float spawnTime = 5;

    void Update()
    {
        if (timeSinceLastSpawn > spawnTime) {
            Enemy enemy = Instantiate(EnemyPrefab).GetComponent<Enemy>();
            enemy.gameObject.SetActive(false);
            enemy.transform.position = transform.position;
            enemy.gameObject.SetActive(true);
            enemy.Init();
            print("EnemySpawned");
            timeSinceLastSpawn = 0;
        }
        timeSinceLastSpawn += Time.deltaTime;
    }
}
