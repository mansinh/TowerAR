using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] float attackRange = 4;
    [SerializeField] float AiUpdateTime = 0.2f;
    [SerializeField] Action attack;

    AIPerception perception;
    float timeSinceAIUpdate = 1;

    private void Start()
    {
        perception = gameObject.AddComponent<AIPerception>();
        perception.setDetectFrom(transform);
        perception.setDetectRange(attackRange);
    }

    void Update()
    {
        timeSinceAIUpdate += Time.deltaTime;
        if (timeSinceAIUpdate > AiUpdateTime)
        {
            Collider closestTarget = perception.getClosestTarget("Enemy");
            print(closestTarget);
            if (closestTarget)
            {
                Destroyable enemyDestroyable = closestTarget.gameObject.GetComponent<Destroyable>();
                if (enemyDestroyable)
                {
                    Attack(enemyDestroyable);
                }
            }
        }
    }


    void Attack(Destroyable other)
    {
        print("attack");
        //transform.LookAt(other.transform);
        attack.Activate(other.transform.position+Vector3.up*other.transform.localScale.y/2);
    }
}
