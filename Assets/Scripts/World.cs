using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class World : MonoBehaviour
{
    
    NavController navController;
    EnemyController enemyController;

    [SerializeField] int levels = 6;
    [SerializeField] float height = 10;
    

    public void Init()
    {
        navController = FindObjectOfType<NavController>();
        navController.Bake();

        enemyController = FindObjectOfType<EnemyController>();
        enemyController.Init(levels, height, this);
    }
    public void Refresh() {
        //navController.Bake();
        NavMeshAgent[] agents = FindObjectsOfType<UnityEngine.AI.NavMeshAgent>();
        foreach (NavMeshAgent agent in agents)
        {
            //agent.ResetPath();
            agent.SetDestination(FindObjectOfType<Statue>().transform.position + Random.onUnitSphere * 2);
        }
    }

   
    
}
