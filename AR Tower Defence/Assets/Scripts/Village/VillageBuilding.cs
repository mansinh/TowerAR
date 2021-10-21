using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuilding : Destroyable
{
 

    protected override void DamageEffects(Damage damage)
    {
        base.DamageEffects(damage);
        //ShakeAnim.StartShake(0.01f, 0.3f, new Vector3(0, -(MaxHealth - Health) / MaxHealth, 0));
        ShakeAnim.StartShake(0.01f, 0.3f, new Vector3(0, 0, 0));
    }
    protected override void Remove()
    {
        base.Remove();
        gameObject.SetActive(false);
    }

}
