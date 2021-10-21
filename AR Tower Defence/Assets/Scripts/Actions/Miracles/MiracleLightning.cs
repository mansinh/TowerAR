using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleLightning : Miracle
{
    public override void Activate()
    {
        base.Activate();
        Collider[] detected = Physics.OverlapSphere(transform.position, Collider.radius);
        foreach (Collider other in detected)
        {
            Destroyable destroyable = other.GetComponent<Destroyable>();

            if (destroyable != null)
            {
               
                if (destroyable.gameObject.layer == 6)
                {
                    destroyable.Damage(MiracleEffect);
                    //destroyable.Shake(0.5f,3);
                }
            }
        }
    }
}
