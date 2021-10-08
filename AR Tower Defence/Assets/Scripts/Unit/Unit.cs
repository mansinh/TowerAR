using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] float attackDamage = 0.1f;
    [SerializeField] float attackCooldown = 3;
    [SerializeField] float attackRange = 4;
    [SerializeField] float AiUpdateTime = 0.2f;
    [SerializeField] ParticleSystem attack;

    AIPerception perception;


    float timeSinceAttack = 0;
    float timeSinceAIUpdate = 0;

    private void Start()
    {
        perception = gameObject.AddComponent<AIPerception>();
        perception.setDetectFrom(attack.transform);
        perception.setDetectRange(attackRange);
    }

    void Update()
    {
        timeSinceAttack += Time.deltaTime;
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
        attack.transform.LookAt(other.transform);
        if (timeSinceAttack > attackCooldown)
        {
            attack.Play();
            timeSinceAttack = 0;
        }
    }

    public float GetAttackDamage() {
        return attackDamage;
    }
}
