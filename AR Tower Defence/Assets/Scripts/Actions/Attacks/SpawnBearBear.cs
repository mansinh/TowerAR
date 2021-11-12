using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that spawns bearbears and inherit's values from the tower in order to be upgradable and update it's values
public class SpawnBearBear : Attack
{
    Pool bearPool;
    [SerializeField] Bear _bearPrefab;
    [SerializeField] int _bearCount;


    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        GameObject bear = bearPool.Push();
        if(bear)
        {
            bear.GetComponent<Bear>().SetDefaultTarget(transform);
        }
       
    }

    private void Start()
    {
        bearPool = gameObject.AddComponent<Pool>();
        bearPool.SetPrefab(_bearPrefab.gameObject);
        bearPool.SetPoolSize(_bearCount);
        bearPool.Init();
    }

    public override void OnUpgrade()
    {
        base.OnUpgrade();
        foreach (GameObject bear in bearPool.Active)
        {
            UpgradeBear(bear.GetComponent<Bear>());
        }
        foreach (GameObject bear in bearPool.Inactive)
        {
            UpgradeBear(bear.GetComponent<Bear>());
        }
    }
    void UpgradeBear(Bear bear)
    {
        (bear._action as Attack)._attackDamageCard = _attackDamageCard;
        (bear._action as Attack)._critDamageCard = _critDamageCard;
        (bear._action as Attack)._critRateCard = _critRateCard;
        (bear._action as Attack)._attackSpeedCard = _attackSpeedCard;
        (bear._action as Attack)._poisonCard = _poisonCard;
        (bear._action as Attack)._slownessCard = _slownessCard;
        (bear._action as Attack)._stunCard = _stunCard;
    }
}
