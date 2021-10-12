using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    [SerializeField] protected float _attackDamage;
    [SerializeField] protected float _criticalDamage;
    [SerializeField] protected float _criticalRate;
    [SerializeField] protected float _stunDamage;
    [SerializeField] protected float _stunRate;
    [SerializeField] protected float _stunDuration;
  

  

    protected Damage CalulateDamage()
    {
        Damage damage = new Damage(_attackDamage, 0.1f);
        if (Random.value < _stunRate)
        {
            damage.damage += _stunDamage;
            damage.stunDuration += _stunDuration;
        }
        if (Random.value < _criticalRate)
        {
            damage.damage *= _criticalDamage;
        }
        Debug.Log("Attack DMG: " + damage.damage + ", Stun duration: " + damage.stunDuration);
        return damage;
    }
}
