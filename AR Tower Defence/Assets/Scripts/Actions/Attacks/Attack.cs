using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class where all damage and attacks calculations are happening as well as cards and modifiers storing
public class Attack : Action
{
    //initial stats input
    [SerializeField] public float _attackDamage;
    [SerializeField] public float _criticalDamage;
    [SerializeField] public float _criticalRate;
    [SerializeField] public float _stunDamage;
    [SerializeField] public float _stunRate;
    [SerializeField] public float _stunDuration;
    [SerializeField] public float _slowness;
    [SerializeField] public float _slownessDuration;
    [SerializeField] public float _poisonDamage;
    [SerializeField] public float _poisonDuration;
    [SerializeField] public float _poisonSlowness;

    //stores base stats

    public float _basicAttackSpeed;
    private float _basicAttackDamage;
    private float _basicCriticalDamage;
    private float _basicCriticalRate;
    private float _basicStunDamage;
    private float _basicStunRate;
    private float _basicStunDuration;
    private float _basicSlowness;
    private float _basicSlownessDuration;
    private float _basicPoisonDamage;
    private float _basicPoisonDuration;
    private float _basicPoisonSlowness;
    private float _attackSpeed;

    public float _attackDamageCard = 0;
    public float _critDamageCard = 0;
    public float _critRateCard = 0;
    public float _slownessCard = 0;
    public float _poisonCard = 0;
    public float _stunCard = 0;
    public float _attackSpeedCard = 0;


    //inherit's base stats in order to store them upon spawning
    protected override void Init()
    {
        base.Init();
        _basicAttackSpeed = cooldown;
        _basicAttackDamage = _attackDamage;
        _basicCriticalDamage = _criticalDamage;
        _basicCriticalRate = _criticalRate;
        _basicStunDamage = _stunDamage;
        _basicStunRate = _stunRate;
        _basicStunDuration = _stunDuration;
        _basicSlowness = _slowness;
        _basicSlownessDuration = _slownessDuration;
        _basicPoisonDamage = _poisonDamage;
        _basicPoisonDuration = _poisonDuration;
        _basicPoisonSlowness = _poisonSlowness;
    }

    //Calculates damage for the tower

    protected Damage CalulateDamage()
    {
        Damage damage = new Damage(_basicAttackDamage, 0, _basicSlowness, _basicSlownessDuration, false, _basicPoisonDamage, _basicPoisonDuration, cooldown);

        

        {_attackDamage = _basicAttackDamage + 7 * _attackDamageCard;}

        {_criticalDamage = _basicCriticalDamage + 0.2f * _critDamageCard;}

        {_criticalRate = (1-(1/((0.15f) * _critRateCard + 1))) + (1 - (1 - (1 / ((0.15f) * _critRateCard + 1)))) * _basicCriticalRate; }
        
        { _slowness = (1-(1/((0.35f* _slownessCard + 1)))) + (1 - (1 - (1 / ((0.35f) * _slownessCard + 1)))) * _basicSlowness;
         _slownessDuration = 3 * Mathf.Min(_stunCard, 1) + _basicSlownessDuration;
        }

        { cooldown = _basicAttackSpeed / (1 + 0.2f * _attackSpeedCard); }

        {_poisonSlowness = (1-(1/((0.2f + _basicPoisonSlowness) * _poisonCard + 1)));
            _poisonDamage = 5 * _poisonCard + _basicPoisonDamage;
            if (_poisonCard > 0) _poisonDuration = 3; else _poisonDuration = 0; }

        
        {_stunRate = (1-(1/((0.15f * _stunCard +1)))) + (1- (1 - (1 / ((0.15f + _basicStunRate) * _stunCard + 1))) * _basicStunRate) * _basicStunRate;
            _stunDamage = 7 * _stunCard + _basicStunDamage;
            _stunDuration = 1* Mathf.Min(_stunCard,1) + _basicStunDuration;
        }

        damage.damage = _attackDamage;

        if (Random.value < _stunRate)
        {
            damage.damage += _stunDamage;
            damage.stunDuration = _stunDuration;
        }

        if (Random.value < _criticalRate)
        {
            damage.damage = damage.damage * _criticalDamage;
            damage.isCritical = true;
        }

        _slowness = _slowness + (1 - _slowness) * _poisonSlowness;


        if (_attackSpeedCard > 0) { damage.attackSpeed = _attackSpeed; }
        if (_poisonDuration > 0) { damage.slowness = _slowness; damage.poisonDamage = _poisonDamage; damage.slownessDuration = 3;damage.poisonDuration = 3; }
        if (_slownessDuration > 0) { damage.slowness = _slowness; damage.slownessDuration = 3; }

       
        return damage;
    }

    public virtual void OnUpgrade()
    {

    }
}

//Object for damage and effect duration's storage
public struct Damage
{
    public float damage;
    public float stunDuration;
    public float slowness;
    public float slownessDuration;
    public float poisonDamage;
    public float poisonDuration;
    public bool isCritical;
    public float attackSpeed;
    public Damage(
        float damage = 0,
        float stunDuration = 0,
        float slowness = 0,
        float slownessDuration = 0,
        bool isCritical = false,
        float poisonDamage = 0,
        float poisonDuration = 0,
        float attackSpeed = 0)
    {
        this.slowness = slowness;
        this.damage = damage;
        this.stunDuration = stunDuration;
        this.slownessDuration = slownessDuration;
        this.isCritical = isCritical;
        this.poisonDamage = poisonDamage;
        this.poisonDuration = poisonDuration;
        this.attackSpeed = attackSpeed;
    }
}