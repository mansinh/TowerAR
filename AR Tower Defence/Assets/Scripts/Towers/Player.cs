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
    protected override void DamageEffects(Damage damage)
    {
      
        base.DamageEffects(damage);
        shakeAnim.StartShake(0.1f, 0.3f, new Vector3(0, -(MaxHealth - Health) / MaxHealth, 0));
    }

    protected override void Remove()
    {
        base.Remove();
        GameController.Instance.GameOver();
    }

}
