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
        
        bearPool.Push();
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
        bear._attack._attackDamageCard = this._attackDamageCard;
        bear._attack._critDamageCard = this._critDamageCard;
        bear._attack._critRateCard = this._critRateCard;
        bear._attack._attackSpeedCard = this._attackSpeedCard;
        bear._attack._poisonCard = this._poisonCard;
        bear._attack._slownessCard = this._slownessCard;
        bear._attack._stunCard = this._stunCard;
    }
}
