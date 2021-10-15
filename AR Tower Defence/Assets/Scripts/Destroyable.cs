using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Destroyable : MonoBehaviour
{
    public NavMeshAgent _navAgent;
    [SerializeField] public Transform _view;
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float DeathDuration;
    [SerializeField] protected float Health;
    [SerializeField] public float _baseSpeed = 1;
    public bool IsDestroyed = false;
    ShakeAnim _shakeAnim;

    private void Awake()
    {
        _shakeAnim = _view.gameObject.AddComponent<ShakeAnim>();
    }

    public virtual void Init()
    {
        IsDestroyed = false;
        Health = MaxHealth;
    }

    public virtual void Damage(Damage damage) {
        Health -= damage.damage;
        DamageAnim(damage);
        if (Health <= 0) {
            StartCoroutine(Die());      
        }
    }

    protected virtual void DamageAnim(Damage damage){
        if (damage.stunDuration > 0)
        {
            _shakeAnim.StartShake(0.1f, 0.1f, Vector3.zero);
        }

        _slownessDuration = Mathf.Max(damage.slownessDuration, _slownessDuration);
        _slowness = Mathf.Max(damage.slowness, _slowness);
        _poisonDuration = Mathf.Max(damage.poisonDuration, _poisonDuration);
        DamagePopup.Create(transform.position, damage, false);

        if (_slownessDuration > 0 && !_isSlowing)
        {
            StartCoroutine(SlownessEffect(damage));
        }

        if (_poisonDuration > 0 && !_isPoisoned)
        {
            StartCoroutine(PoisonEffect(damage));
        }

        StartCoroutine(Stun(damage.stunDuration));
    }

    float _slowness;
    float _slownessDuration;
    float _poisonDuration;
    bool _isSlowing = false;
    bool _isPoisoned = false;
    IEnumerator SlownessEffect(Damage damage)
    {

        _isSlowing = true;

        while (_slownessDuration > 0)
        {
            _slownessDuration -= Time.deltaTime;
            _navAgent.speed = _baseSpeed * (1f - _slowness);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _isSlowing = false;
        _slownessDuration = 0;
        _slowness = 0;
        _navAgent.speed = _baseSpeed;
    }

    IEnumerator PoisonEffect(Damage damage)
    {
        _isPoisoned = true;   
        while(_poisonDuration > 0)
        {
            Health -= damage.poisonDamage;
            DamagePopup.Create(transform.position, damage, _isPoisoned);
            _poisonDuration--;
            yield return new WaitForSeconds(1);
        }
        _isPoisoned = false;
        _poisonDuration = 0;
    }

    IEnumerator Stun(float duration)
    {
        _navAgent.isStopped = true;
        yield return new WaitForSeconds(duration);
        _navAgent.isStopped = false;
    }

    protected virtual void Death()
    {
        _isSlowing = false;
        _isPoisoned = false;
        IsDestroyed = true;
    }


    protected virtual void DeathAnim(){}

    IEnumerator Die()
    {
        DeathAnim();
        yield return new WaitForSeconds(DeathDuration);
        Death();
    }

}

public struct Damage
{
    public float damage;
    public float stunDuration;
    public float slowness;
    public float slownessDuration;
    public float poisonDamage;
    public float poisonDuration;
    public bool isCritical;
    public Damage(
        float damage = 0,
        float stunDuration = 0, 
        float slowness = 0, 
        float slownessDuration = 0,
        bool isCritical = false,
        float poisonDamage = 0, 
        float poisonDuration = 0)
    {
        this.slowness = slowness;
        this.damage = damage;
        this.stunDuration = stunDuration;
        this.slownessDuration = slownessDuration;
        this.isCritical = isCritical;
        this.poisonDamage = poisonDamage;
        this.poisonDuration = poisonDuration;
    }
}


