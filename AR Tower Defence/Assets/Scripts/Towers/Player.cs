using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destroyable
{

    ShakeAnim shakeAnim;


    private void Start()
    {
        Init();
    
        shakeAnim = _view.gameObject.AddComponent<ShakeAnim>();
    }
    protected override void DamageAnim(Damage damage)
    {
      
        base.DamageAnim(damage);
        shakeAnim.StartShake(0.1f, 0.3f, new Vector3(0, -(MaxHealth - Health) / MaxHealth, 0));
        //StartCoroutine(ShowHealth());
       
        
        
    }

    protected override void Death()
    {
        base.Death();
    }

}
