using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destroyable
{
    [SerializeField] Transform _view;

    ShakeAnim shakeAnim;


    private void Start()
    {
        Init();
    
        shakeAnim = _view.gameObject.AddComponent<ShakeAnim>();
    }
    protected override void DamageAnim(float damage)
    {
      
        base.DamageAnim(damage);
        StartCoroutine(ShowHealth());
        shakeAnim.StartShake(0.1f,0.3f, _view.localPosition);
        
        
    }
    IEnumerator ShowHealth() {
        yield return new WaitForSeconds(0.3f);        
        _view.localPosition = new Vector3(0, -(MaxHealth -Health) / MaxHealth, 0);
        
    }
}
