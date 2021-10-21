using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class Agent : Destroyable
{
    [SerializeField] public float _baseSpeed = 1;
    [SerializeField] protected string Name = "";
    [SerializeField] Attack _attack;
    [SerializeField] float _detectRange;
    [SerializeField] float AiUpdateTime = 1f;
    [SerializeField] protected Transform DefaultTarget;
    [SerializeField] protected string TargetName = "";
    [SerializeField] protected float _distanceFromTarget=1;
    NavMeshAgent _navAgent;


    AIPerception perception;
    [SerializeField] float timeSinceAIUpdate = 1;
    [SerializeField] Transform _currentTarget;

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
        OnSpawn();
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
        SetTarget(DefaultTarget);
    }

    public void SetTarget(Transform target)
    {
        //if (_currentTarget != target || _currentTarget==null)
        //{
            if (_navAgent.isOnNavMesh)
            {
                _currentTarget = target;

                Vector3 randomDisplacement =  Random.onUnitSphere / 4* _distanceFromTarget;
                randomDisplacement.y = 0;
                if (target)
                {
                    _navAgent.SetDestination(target.transform.position + randomDisplacement);
                }
                else {
                    _navAgent.SetDestination(transform.position + randomDisplacement);
                }
            }
        //}
    }
    protected override void Death()
    {
        _attack.EndAction();
        _navAgent.speed = 0;
        _navAgent.ResetPath();
        base.Death();      
    }
    protected override void Remove()
    {
       
        base.Remove();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (_currentTarget)
        {
            if (_attack.Activate(_currentTarget.position + Vector3.up * _currentTarget.transform.localScale.y / 2))
            {
                transform.LookAt(_currentTarget);
            }
        }
        timeSinceAIUpdate += Time.deltaTime;
        if (timeSinceAIUpdate > AiUpdateTime)
        {
            Destroyable closestTarget = perception.getClosestTarget(TargetName);

            if (closestTarget)
            {              
                SetTarget(closestTarget.transform);   
            }
            else if(Random.value < 0.5)
            {
                SetTarget(DefaultTarget);
            }
            timeSinceAIUpdate = 0;
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
    protected override void OnEndStun()
    {
        _navAgent.isStopped = false;
    }
}
