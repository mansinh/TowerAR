using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Destroyable
{
    EnemySource _source;
    [SerializeField] string _name = "enemy";
    [SerializeField] Attack _attack;
    [SerializeField] float _detectRange;
    [SerializeField] float AiUpdateTime = 0.2f;
    AIPerception perception;
    float timeSinceAIUpdate = 1;

    

    private void Start()
    {
        perception = gameObject.AddComponent<AIPerception>();
        perception.setDetectFrom(transform);
        perception.setDetectRange(_detectRange);
    }

    public void Init(EnemySource source)
    {
        base.Init();
        _source = source;
        _navAgent = GetComponent<NavMeshAgent>();
        transform.localScale = source.transform.localScale;
    }

    public void Spawn()
    {
        _navAgent.speed = _baseSpeed;
        _navAgent.isStopped = false;
        Init();
        Debug.Log(_navAgent.speed + " " + _baseSpeed);
        FindDestination();
    }

    public void FindDestination()
    {
        Vector3 randomDisplacement = Random.onUnitSphere / 4 * WorldRoot.instance.transform.localScale.x;
        randomDisplacement.y = 0;
        _navAgent.SetDestination(FindObjectOfType<Player>().transform.position + randomDisplacement);
    }

    protected override void Death()
    {
        _attack.EndAction();
        _navAgent.isStopped = true;
        _source.OnEnemyDeath(this);
        base.Death();

        Points.instance.EnemyKilled(_name);
    }

    protected override void DamageAnim(Damage damage)
    {
        base.DamageAnim(damage);

        
    }

    void Update()
    {
        timeSinceAIUpdate += Time.deltaTime;
        if (timeSinceAIUpdate > AiUpdateTime)
        {
            Collider closestTarget = perception.getClosestTarget("Player");

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
        //print("ATTACK PLAYER");
        transform.LookAt(other.transform);
        _attack.Activate(other.transform.position + Vector3.up * other.transform.localScale.y / 2);
    }
}
 

