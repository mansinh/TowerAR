using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Destroyable
{
    NavMeshAgent _navAgent;
    EnemySource _source;
    [SerializeField] Transform _view;
    [SerializeField] string _name = "basic";
    [SerializeField] float _speed;
    [SerializeField] float _attackDamage = 0.1f;
    [SerializeField] float _attackCooldown = 0.03f;


    ShakeAnim _shakeAnim;
    float _timeSinceAttack = 0;


    private void Awake()
    {
        _shakeAnim = _view.gameObject.AddComponent<ShakeAnim>();
        
    }

    public void Init(EnemySource source)
    {
        base.Init();
        _source = source;
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = _speed;
 
        transform.localScale = source.transform.localScale;
    }

    public void Spawn() {
        _navAgent.isStopped = false;
        Init();
        FindDestination();
    }

    public void FindDestination() {
        _navAgent.SetDestination(FindObjectOfType<Player>().transform.position + Random.onUnitSphere/2 * WorldRoot.instance.transform.localScale.x);
    }

    protected override void Death()
    {
        _navAgent.isStopped = true;
        _source.OnEnemyDeath(this);
        base.Death();

        Points.instance.EnemyKilled(_name);
    }

    protected override void DamageAnim(float damage)
    {
        base.DamageAnim(damage);
        _shakeAnim.StartShake(0.1f,0.1f,Vector3.zero);
        StartCoroutine(Stun(0.3f));
    }

 
    IEnumerator Stun(float duration) {
        _navAgent.isStopped = true;
        yield return new WaitForSeconds(duration);
        _navAgent.isStopped = false;
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
        _timeSinceAttack += Time.deltaTime;
    }

    void Attack(Destroyable other) {
        if (_timeSinceAttack > _attackCooldown)
        {
            other.Damage(_attackDamage);
            _timeSinceAttack = 0;
        }
    }

 
}
