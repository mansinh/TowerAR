using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<EnemySource> sources = new List<EnemySource>();
    [SerializeField] GameObject enemySourcePrefab;
    [SerializeField] float spawnTime;
   
    public void Init(int levels, float height, World world)
    {
        List<Vector3> randomPoints = new List<Vector3>();
        for (float i = 0; i < levels; i++)
        {
            float offset = Random.value * Mathf.PI * 2;
            float sourceCount = levels - i - 1;
            float gap = Mathf.PI * 2 / sourceCount;
            for (int j = 0; j < sourceCount; j++)
            {
                float angle = offset + (j + Random.value - 0.5f) * gap;
                float x = 10 * Mathf.Cos(angle);
                float y = (i / levels + Random.value / levels / 2) * height;
                float z = 10 * Mathf.Sin(angle);

                Vector3 randomPoint = new Vector3(x, y, z);
                randomPoints.Add(randomPoint);
            }
        }

        sources = new List<EnemySource>();
        for (int i = 0; i < randomPoints.Count; i++)
        {
            RaycastHit hit;
            Vector3 direction = -new Vector3(randomPoints[i].x, 0, randomPoints[i].z);
           
            Physics.Raycast(randomPoints[i] + transform.position, direction, out hit);
            EnemySource source = Instantiate(enemySourcePrefab, world.transform).GetComponent<EnemySource>();
            source.transform.position = hit.point;
            
            source.transform.right = hit.normal;
            sources.Add(source);
            source.Init();  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
