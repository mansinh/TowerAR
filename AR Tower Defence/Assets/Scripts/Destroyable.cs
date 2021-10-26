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
        Poison(damage);
        DamagePopup.Create(transform, damage, false);
        Slow(damage.slownessDuration, damage.slowness);
        if (damage.stunDuration > 0) { StartCoroutine(Stun(damage.stunDuration)); }
        UpdateView();
    }


    float _slowness;
    float _slownessDuration;
    bool _isSlowing = false;
    public void Slow(float slownessDuration, float slowness)
    {
        _slownessDuration = Mathf.Max(slownessDuration, _slownessDuration);
        _slowness = Mathf.Max(slowness, _slowness);
        if (_slownessDuration > 0 && !_isSlowing)
        {
            StartCoroutine(SlownessEffect());
        }
    }

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


    float _poisonDuration;
    float _poisonDamage;
    bool _isPoisoned = false;

    protected void Poison(Damage damage)
    {
        _poisonDuration = Mathf.Max(damage.poisonDuration, _poisonDuration);
        _poisonDamage = Mathf.Max(damage.poisonDamage, _poisonDamage);

        if (_poisonDuration > 0 && !_isPoisoned)
        {
            StartCoroutine(PoisonEffect(damage));
        }
    }

    IEnumerator PoisonEffect(Damage damage)
    {
        _isPoisoned = true;   
        while(_poisonDuration > 0)
        {
            Health -= _poisonDamage;
            if (damage.poisonDamage != _poisonDamage) damage.poisonDamage = _poisonDamage;
            DamagePopup.Create(transform, damage, _isPoisoned);
            _poisonDuration--;
            yield return new WaitForSeconds(1);
        }
        _isPoisoned = false;
        _poisonDuration = 0;
        _poisonDamage = 0;
    }

    IEnumerator Stun(float duration)
    {
        ShakeAnim.StartShake(0.1f, 0.3f, Vector3.zero);
        OnStun();
        yield return new WaitForSeconds(duration);
        OnEndStun();
    }

    public void Shake(float amplitude, float duration) {
        ShakeAnim.StartShake(0.1f, 0.3f, Vector3.zero);
    }
    protected virtual void OnStun() { }
    protected virtual void OnEndStun() { }

    protected virtual void Remove()
    {
        _isSlowing = false;
        _isPoisoned = false;
        IsDestroyed = true;
        _slownessDuration = 0;
        _slowness = 0;
        _poisonDuration = 0;
        _poisonDamage = 0;
    }


    protected virtual void Death(){}

    IEnumerator Die()
    {
        Death();
        yield return new WaitForSeconds(DeathDuration);
        Remove();
    }

    protected virtual void UpdateView()
    {
       
    }
}