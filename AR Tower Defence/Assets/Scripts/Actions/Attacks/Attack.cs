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
    [SerializeField] protected float _slowness;
    [SerializeField] protected float _slownessDuration;
    [SerializeField] protected float _poisonDamage;
    [SerializeField] protected float _poisonDuration;
    [SerializeField] protected float _poisonSlowness;
  

  

    protected Damage CalulateDamage()
    {
        Damage damage = new Damage(_attackDamage, 0, _slowness, _slownessDuration, false, _poisonDamage, _poisonDuration);

        if (Random.value < _stunRate)
        {
            damage.damage += _stunDamage;
            damage.stunDuration += _stunDuration;
        }
        if (Random.value < _criticalRate)
        {
            damage.damage *= _criticalDamage;
            damage.isCritical = true;
        }

        if (_slowness > 0 && _poisonSlowness > 0)
        {
            damage.slowness = (1 - 1*(1 + (_slowness+_poisonSlowness))) * - 1;
        }
        else if (_slowness > 0)
        {
            damage.slowness = (1 - 1 * (1 + _slowness)) * -1;
        }
        else if (_poisonSlowness > 0)
        {
            damage.slowness = (1 - 1 * (1 + _poisonSlowness)) * - 1;
        }
        return damage;
    }
}
