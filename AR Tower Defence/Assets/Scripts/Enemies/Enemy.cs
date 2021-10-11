using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Destroyable
{
    NavMeshAgent navAgent;
    EnemySource source;
    [SerializeField] float speed;
    [SerializeField] float attackDamage = 0.1f;
    [SerializeField] float attackCooldown = 0.03f;
    ShakeAnim shakeAnim;

    float timeSinceAttack = 0;

    private void Awake()
    {
        shakeAnim = gameObject.AddComponent<ShakeAnim>();
        
    }

    public void Init(EnemySource source)
    {
        base.Init();
        this.source = source;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
 
        transform.localScale = source.transform.localScale;
    }

    public void Spawn() {
        navAgent.isStopped = false;
        Init();
        FindDestination();
    }

    public void FindDestination() {
        navAgent.SetDestination(FindObjectOfType<Player>().transform.position + Random.onUnitSphere/4 * WorldRoot.instance.transform.localScale.x);
    }

    protected override void Death()
    {
        navAgent.isStopped = true;
        source.OnEnemyDeath(this);
        base.Death();

    }

    protected override void DamageAnim(float damage)
    {
        base.DamageAnim(damage);
        shakeAnim.StartShake(0.1f,0.1f);
        StartCoroutine(Stun(0.3f));
    }

 
    IEnumerator Stun(float duration) {
        navAgent.isStopped = true;
        yield return new WaitForSeconds(duration);
        navAgent.isStopped = false;
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
