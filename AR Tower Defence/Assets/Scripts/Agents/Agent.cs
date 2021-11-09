using UnityEngine;
using UnityEngine.AI;

/**
 * Destroyable that chases targets within detect range 
 * Goes to default target if no targets in range if there is one, otherwise wanders randomly
 *@ author Manny Kwong 
 */


[RequireComponent(typeof(NavMeshAgent))]

public class Agent : Destroyable
{
    public float BaseSpeed = 1;
    [SerializeField] protected string Name = "";
    [SerializeField] public Action _action;
    [SerializeField] float DetectRange = 3;
    [SerializeField] protected float AiUpdateTime = 1f;
    [SerializeField] protected Transform DefaultTarget;
    [SerializeField] protected string TargetName = "";
    [SerializeField] protected float DistanceFromTarget=1;
    [SerializeField] protected float MaxHeightDiff = 0.01f;

   
    private NavMeshAgent _navAgent;


    [SerializeField] protected AIPerception Perception;
    protected float TimeSinceAIUpdate = 1;
    protected Transform CurrentTarget;

  



    protected override void Init()
    {
        if (Perception == null)
        {
            Perception = gameObject.AddComponent<AIPerception>();
            Perception.setDetectFrom(transform);
            Perception.setDetectRange(DetectRange);
        }
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
        _navAgent.speed = BaseSpeed;
        _navAgent.isStopped = false;
        SetTarget(DefaultTarget,DistanceFromTarget);
    }

    public void SetTarget(Transform target, float distanceFromTarget)
    {
        if (CurrentTarget != target){
            if (_navAgent.isOnNavMesh)
            {
                CurrentTarget = target;

                //Set target location to a random location near target  
                Vector3 randomDisplacement =  Random.onUnitSphere / 4* distanceFromTarget*World.Instance.transform.localScale.x;
                randomDisplacement.y = 0;
                if (target)
                {
                    _navAgent.SetDestination(target.transform.position + randomDisplacement);
                }
                else {
                    _navAgent.SetDestination(transform.position + randomDisplacement);
                }
            }
        }
    }

    public void SetDefaultTarget(Transform target)
    {
        DefaultTarget = target;
    }

    protected override void Death()
    {
        if (_action)
        {
            _action.EndAction();
        }
        if (_navAgent)
        {
            _navAgent.speed = 0;
            //Reset nav data on death
            _navAgent.ResetPath();
        }
        base.Death();      
    }

    protected override void Remove()
    {
       
        base.Remove();
        gameObject.SetActive(false);
    }

    void Update()
    {     
        LookAround();
        Act();
    }

    protected virtual void Act()
    {
        //Turn towards current target and attack if in range and cooldown over
        if (CurrentTarget && _action != null)
        {
            if (_action.Activate(CurrentTarget.position + Vector3.up * CurrentTarget.transform.localScale.y / 2))
            {
                transform.LookAt(CurrentTarget);
            }
        }
    }

    protected virtual void LookAround()
    {
        //Periodically decide on target, not every frame as that may be expensive
        TimeSinceAIUpdate += Time.deltaTime;
        if (TimeSinceAIUpdate > AiUpdateTime)
        {
            Destroyable closestTarget = Perception.getClosestTarget(TargetName, MaxHeightDiff);

            if (closestTarget)
            {
                SetTarget(closestTarget.transform, DistanceFromTarget);

            }
            else if (Random.value < 0.1 && DefaultTarget != null)
            {
                SetTarget(DefaultTarget, DistanceFromTarget);
            }
            TimeSinceAIUpdate = 0;
        }

        //If current target is inactive turn off
        if (CurrentTarget)
        {
            if (!CurrentTarget.gameObject.active)
            {
                SetTarget(DefaultTarget, DistanceFromTarget);
            }
        }
    }

    //Slow down navagent movement when affected by slow condition
    protected override void OnSlow(float slowness)
    {
        _navAgent.speed = BaseSpeed * (1f - slowness);
    }
    protected override void OnEndSlow()
    {
        _navAgent.speed = BaseSpeed;
    }

    //Stop navagent when affected by stun condition
    protected override void OnStun()
    {
        _navAgent.isStopped = true;
    }
    protected override void OnEndStun()
    {
        _navAgent.isStopped = false;
    }

    public Vector3 GetVelocityFraction()
    {
        return _navAgent.velocity / _navAgent.speed;
    }
}
