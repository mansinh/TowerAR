using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class Agent : Destroyable
{

    [SerializeField] protected string Name = "";
    [SerializeField] Attack _attack;
    [SerializeField] float _detectRange;
    [SerializeField] float AiUpdateTime = 1f;
    [SerializeField] protected Transform DefaultTarget;
    [SerializeField] protected string TargetName = "";
    NavMeshAgent _navAgent;
 

    AIPerception perception;
    float timeSinceAIUpdate = 1;
    Destroyable targetDestroyable;

    private void Start()
    {
        perception = gameObject.AddComponent<AIPerception>();
        perception.setDetectFrom(transform);
        perception.setDetectRange(_detectRange);
    }

    protected override void Init()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        base.Init();
        
    }

    void OnEnable()
    {
        OnSpawn();
    }

    public virtual void OnSpawn()
    {
        IsDestroyed = false;
        Health = MaxHealth;
        _navAgent.speed = _baseSpeed;
        _navAgent.isStopped = false;       
    }

    public void SetDestination(Vector3 destination)
    {
       
        if (_navAgent.isOnNavMesh)
        {
            Vector3 randomDisplacement = Random.onUnitSphere / 4 * WorldRoot.instance.transform.localScale.x;
            randomDisplacement.y = 0;
            _navAgent.SetDestination(destination + randomDisplacement);
        }
    }

    protected override void Death()
    {
        _attack.EndAction();
        _navAgent.isStopped = true;
        base.Death();
        gameObject.SetActive(false);
    }

    void Update()
    {
        timeSinceAIUpdate += Time.deltaTime;
        if (timeSinceAIUpdate > AiUpdateTime)
        {
            Collider closestTarget = perception.getClosestTarget(TargetName);

            if (closestTarget)
            {
                targetDestroyable = closestTarget.gameObject.GetComponent<Destroyable>();
                if (targetDestroyable)
                {           
                    // Attack if in attack range else move towards target 
                    if (_attack.Activate(targetDestroyable.transform.position + Vector3.up * targetDestroyable.transform.localScale.y / 2))
                    {
                        transform.LookAt(targetDestroyable.transform);
                    }
                    else
                    {
                        SetDestination(targetDestroyable.transform.position);
                    }


                }
            }
            else if(DefaultTarget){
                SetDestination(DefaultTarget.position);
            }

        }

    }

    protected override void OnSlow(float slowness)
    {
        _navAgent.speed = _baseSpeed * (1f - slowness);
    }
    protected override void OnEndSlow()
    {
        _navAgent.speed = _baseSpeed;
    }

    protected override void OnStun()
    {
        _navAgent.isStopped = true;
    }
    protected override  void OnEndStun()
    {
        _navAgent.isStopped = false;
    }
}
