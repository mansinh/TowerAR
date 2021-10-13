using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float DeathDuration;
    [SerializeField] protected float Health;
    public bool IsDestroyed = false;

    
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

    protected virtual void DamageAnim(Damage damage){}
    protected virtual void Death()
    {
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
    public Damage(float damage = 0,float stunDuration = 0, float slowness = 0, float slownessDuration = 0)
    {
        this.slowness = slowness;
        this.damage = damage;
        this.stunDuration = stunDuration;
        this.slownessDuration = slownessDuration;
    }
}


