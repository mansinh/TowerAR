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
    public enum AgentState
    {
        Action0,
        Action1,
        Running,
        Idling
    }

    public float BaseSpeed = 1;
    [SerializeField] protected string Name = "";
    [SerializeField] public Action _action;
    [SerializeField] float DetectRange = 3;
    [SerializeField] protected float AiUpdateTime = 1f;
    [SerializeField] protected Transform DefaultTarget;
    [SerializeField] protected string TargetName = "";
    [SerializeField] protected float DistanceFromTarget=1;
    [SerializeField] protected float MaxHeightDiff = 0.01f;
    [SerializeField] protected Animator animator;
    public AudioSource SoundEffects;
    protected NavMeshAgent NavAgent;
    public AgentState State;

    [SerializeField] protected AIPerception Perception;
    protected float TimeSinceAIUpdate = 1;
    protected Transform CurrentTarget;

    protected override void Init()
    {
        SoundEffects = gameObject.AddComponent<AudioSource>();
        if (Perception == null)
        {
            Perception = gameObject.AddComponent<AIPerception>();
            Perception.setDetectFrom(transform);
            Perception.setDetectRange(DetectRange);
        }
        NavAgent = GetComponent<NavMeshAgent>();
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
        NavAgent.speed = BaseSpeed;
        NavAgent.isStopped = false && NavAgent.isOnNavMesh;
        SetTarget(DefaultTarget,DistanceFromTarget);
        if (_action)
        {
            _action.enabled = true;
        }
    }

    public void SetTarget(Transform target, float distanceFromTarget)
    {
        if (CurrentTarget != target){
            if (NavAgent.isOnNavMesh)
            {
                CurrentTarget = target;

                //Set target location to a random location near target  
                Vector3 randomDisplacement = GetRandomAround(distanceFromTarget);
                randomDisplacement.y = 0;
                if (target)
                {
                    NavAgent.SetDestination(target.transform.position + randomDisplacement);
                }
                else {
                    NavAgent.SetDestination(transform.position + randomDisplacement);
                }
            }
        }
    }

    public Vector3 GetRandomAround(float distanceFromTarget)
    {
        Vector2 randomCircle = Random.insideUnitCircle;
        return new Vector3(randomCircle.x,0,randomCircle.y) * distanceFromTarget * World.Instance.transform.localScale.x;
    }

    public void SetDefaultTarget(Transform target)
    {
        DefaultTarget = target;
    }

    protected override void Death()
    {
        if (animator)
        {
            animator.SetTrigger("Die");
        }
        if (_action)
        {
            _action.EndAction();
            _action.enabled = false;
        }
        if (NavAgent)
        {
            NavAgent.speed = 0;
            //Reset nav data on death
            NavAgent.ResetPath();
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
        Vector3 velocity = GetVelocityFraction();
        if (velocity.sqrMagnitude > 0.01)
        {
            if (animator)
            {
                animator.SetBool("Run Forward", true);
            }
            State = AgentState.Running;
        }
        else
        {
            if (animator)
            {
                animator.SetBool("Run Forward", false);
            }
            State = AgentState.Idling;
        }

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
                if (animator)
                {
                    animator.SetTrigger("Attack 01");
                }
                State = AgentState.Action0;
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
                print("CLOSEST TARGET " + closestTarget);
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

    protected override void DamageEffects(Damage damage)
    {
        
        if (animator)
        {
            animator.SetTrigger("Take Damage");
        }
        base.DamageEffects(damage);
    }


    //Slow down navagent movement when affected by slow condition
    protected override void OnSlow(float slowness)
    {
        NavAgent.speed = BaseSpeed * (1f - slowness);
    }
    protected override void OnEndSlow()
    {
        NavAgent.speed = BaseSpeed;
    }

    //Stop navagent when affected by stun condition
    protected override void OnStun()
    {
        NavAgent.isStopped = true;
    }
    protected override void OnEndStun()
    {
        NavAgent.isStopped = false;
    }

    public Vector3 GetVelocityFraction()
    {
        return NavAgent.velocity / NavAgent.speed;
    }
}
