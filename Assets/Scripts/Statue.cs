using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : Destroyable
{
    [SerializeField] Transform mesh;
    [SerializeField] float showHealthDuration = 0.1f;
    ShakeAnim shakeAnim;


    private void Start()
    {
        Init();
        shakeAnim = mesh.GetComponent<ShakeAnim>();
    }
    protected override void DamageAnim(float damage)
    {
      
        base.DamageAnim(damage);
        StartCoroutine(ShowHealth());
        shakeAnim.StartShake();
        
        
    }
    IEnumerator ShowHealth() {
        yield return new WaitForSeconds(showHealthDuration);        
        mesh.localPosition = new Vector3(0, -(maxHealth -health) / maxHealth, 0);
    }
}
