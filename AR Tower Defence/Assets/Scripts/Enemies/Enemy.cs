using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Destroyable
{
    NavMeshAgent _navAgent;
    EnemySource _source;
    [SerializeField] Transform _view;
    [SerializeField] string _name = "enemy";
    [SerializeField] float _speed;
    [SerializeField] Attack _attack;
    [SerializeField] float _detectRange;
    [SerializeField] float _AiUpdateTime = 0.2f;

    ShakeAnim _shakeAnim;
    AIPerception perception;
    float timeSinceAIUpdate = 1;

    private void Awake()
    {
        _shakeAnim = _view.gameObject.AddComponent<ShakeAnim>();

    }

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
        _navAgent.speed = _speed;

        transform.localScale = source.transform.localScale;
    }

    public void Spawn()
    {
        _navAgent.isStopped = false;
        Init();
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
        _navAgent.isStopped = true;
        _source.OnEnemyDeath(this);
        base.Death();

        Points.instance.EnemyKilled(_name);
    }

    protected override void DamageAnim(Damage damage)
    {
        base.DamageAnim(damage);
        _shakeAnim.StartShake(0.1f,0.1f, Vector3.zero);
        StartCoroutine(Stun(damage.stunDuration));
    }


    IEnumerator Stun(float duration)
    {
        _navAgent.isStopped = true;
        yield return new WaitForSeconds(duration);
        _navAgent.isStopped = false;
    }



    void Update()
    {
        timeSinceAIUpdate += Time.deltaTime;
        if (timeSinceAIUpdate > _AiUpdateTime)
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
        print("ATTACK PLAYER");
        transform.LookAt(other.transform);
        _attack.Activate(other.transform.position + Vector3.up * other.transform.localScale.y / 2);
    }
}
 

