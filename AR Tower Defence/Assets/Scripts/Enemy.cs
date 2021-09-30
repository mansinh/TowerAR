using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Destroyable
{
    NavMeshAgent navAgent;
    EnemySource source;
    [SerializeField] float speed, acceleration;
    [SerializeField] float attackDamage = 0.1f;
    [SerializeField] float attackCooldown = 3;
    

    float timeSinceAttack = 0;

    public void Init(EnemySource source)
    {
        base.Init();
        this.source = source;
        navAgent = GetComponent<NavMeshAgent>();
        
    }

    public void Spawn() {
        navAgent.isStopped = false;
        Init();
        navAgent.SetDestination(FindObjectOfType<Statue>().transform.position+Random.onUnitSphere*2);
    }

    protected override void Death()
    {
        navAgent.isStopped = true;
        source.OnEnemyDeath(this);
        base.Death();
        
    }

    private void OnTriggerStay(Collider other)
    {
        string hitTag = other.tag;

        switch (hitTag)
        {
            case "Player":
                Attack(other.attachedRigidbody.gameObject.GetComponent<Destroyable>());
                break;
            default:
                
                break;
        }
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
    }

    void Attack(Destroyable other) {
        if (timeSinceAttack > attackCooldown)
        {
            other.Damage(attackDamage);
            timeSinceAttack = 0;
        }
    }
}
