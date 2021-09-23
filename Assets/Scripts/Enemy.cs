using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navAgent;
    [SerializeField] float speed, acceleration;

  
    public void Init()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.SetDestination(FindObjectOfType<Statue>().transform.position);
    }

    void Update()
    {
        if (navAgent.isOnOffMeshLink)
        {
            if (navAgent.speed > speed / 4)
            {
                //navAgent.speed -= acceleration * Time.deltaTime;
            }

        }
        else {
            if (navAgent.speed < speed)
            {
                //navAgent.speed += acceleration * Time.deltaTime;
            }
        }
    }
}
