using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Destroyable : MonoBehaviour
{
 
    [SerializeField] public Transform _view;
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float DeathDuration;
    [SerializeField] protected float Health;
    [SerializeField] public float _baseSpeed = 1;
    public bool IsDestroyed = false;
    protected ShakeAnim ShakeAnim;

    protected void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        ShakeAnim = _view.gameObject.AddComponent<ShakeAnim>();
        IsDestroyed = false;
        Health = MaxHealth;

    }
   
   
    public virtual void Damage(Damage damage) {
        Health -= damage.damage;
        DamageEffects(damage);
        if (Health <= 0) {
            StartCoroutine(Die());      
        }
    }

    protected virtual void DamageEffects(Damage damage){
        if (damage.stunDuration > 0)
        {
            ShakeAnim.StartShake(0.1f, 0.1f, Vector3.zero);
        }

        _slownessDuration = Mathf.Max(damage.slownessDuration, _slownessDuration);
        _slowness = Mathf.Max(damage.slowness, _slowness);
        _poisonDuration = Mathf.Max(damage.poisonDuration, _poisonDuration);
        DamagePopup.Create(transform, damage, false);

        if (_slownessDuration > 0 && !_isSlowing)
        {
            StartCoroutine(SlownessEffect());
        }

        if (_poisonDuration > 0 && !_isPoisoned)
        {
            StartCoroutine(PoisonEffect(damage));
        }

        StartCoroutine(Stun(damage.stunDuration));
    }

    float _slowness;
    float _slownessDuration;
    bool _isSlowing = false;
    
    IEnumerator SlownessEffect()
    {
        _isSlowing = true;
        while (_slownessDuration > 0)
        {
            _slownessDuration -= Time.deltaTime;
            OnSlow(_slowness);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _isSlowing = false;
        _slownessDuration = 0;
        _slowness = 0;
    }

    protected virtual void OnSlow(float slowness){}
    protected virtual void OnEndSlow(){}



    IEnumerator Stun(float duration)
    {
        ShakeAnim.StartShake(0.1f, 0.3f, Vector3.zero);
        OnStun();
        yield return new WaitForSeconds(duration);
        OnEndStun();
    }

    protected virtual void OnStun(){}
    protected virtual void OnEndStun(){}


    float _poisonDuration;
    bool _isPoisoned = false;
    IEnumerator PoisonEffect(Damage damage)
    {
        _isPoisoned = true;   
        while(_poisonDuration > 0)
        {
            Health -= damage.poisonDamage;
            DamagePopup.Create(transform, damage, _isPoisoned);
            _poisonDuration--;
            yield return new WaitForSeconds(1);
        }
        _isPoisoned = false;
        _poisonDuration = 0;
    }

   

    protected virtual void Remove()
    {
        _isSlowing = false;
        _isPoisoned = false;
        IsDestroyed = true;
    }


    protected virtual void Death(){}

    IEnumerator Die()
    {
        Death();
        yield return new WaitForSeconds(DeathDuration);
        Remove();
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


