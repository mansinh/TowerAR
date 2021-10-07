using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldRoot : MonoBehaviour
{
    public static WorldRoot instance;

    NavController navController;
    EnemyController enemyController;

    [SerializeField] int levels = 6;
    [SerializeField] float height = 10;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        navController = FindObjectOfType<NavController>();
       

        //enemyController = FindObjectOfType<EnemyController>();
        //enemyController.Init(levels, height, this);
    }
    public void Refresh() {
        navController.Bake();
        NavMeshAgent[] agents = FindObjectsOfType<UnityEngine.AI.NavMeshAgent>();
        foreach (NavMeshAgent agent in agents)
        {
            //agent.ResetPath();
            
        }
    }

   
    
}
