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
    [SerializeField] public Attack _attack;
    [SerializeField] float DetectRange = 3;
    [SerializeField] float AiUpdateTime = 1f;
    [SerializeField] protected Transform DefaultTarget;
    [SerializeField] protected string TargetName = "";
    [SerializeField] protected float DistanceFromTarget=1;
    [SerializeField] protected float maxHeightDiff = 0.01f;
    private NavMeshAgent _navAgent;


    private AIPerception _perception;
    private float timeSinceAIUpdate = 1;
    private Transform _currentTarget;

    private void Start()
    {
        _perception = gameObject.AddComponent<AIPerception>();
        _perception.setDetectFrom(transform);
        _perception.setDetectRange(DetectRange);
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
        _navAgent.speed = BaseSpeed;
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

                //Set target location to a random location near target  
                Vector3 randomDisplacement =  Random.onUnitSphere / 4* DistanceFromTarget;
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
        //Reset nav data on death
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
        //Turn towards current target and attack if in range and cooldown over
        if (_currentTarget)
        {
            if (_attack.Activate(_currentTarget.position + Vector3.up * _currentTarget.transform.localScale.y / 2))
            {
                transform.LookAt(_currentTarget);
            }
        }

        //Periodically decide on target, not every frame as that may be expensive
        timeSinceAIUpdate += Time.deltaTime;
        if (timeSinceAIUpdate > AiUpdateTime)
        {
            Destroyable closestTarget = _perception.getClosestTarget(TargetName, maxHeightDiff);

            if (closestTarget)
            {              
                SetTarget(closestTarget.transform);   
            }
            else if(Random.value < 0.1)
            {
                //Random chance to go back to default target
                SetTarget(DefaultTarget);
            }
            timeSinceAIUpdate = 0;
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
}
