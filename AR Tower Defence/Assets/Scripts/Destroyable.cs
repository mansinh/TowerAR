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

    public virtual void Damage(float damage)
    {
        Health -= damage;
        DamageAnim(damage);
        if (Health <= 0) {
            StartCoroutine(Die());      
        }
    }

    protected virtual void DamageAnim(float damage){}
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


