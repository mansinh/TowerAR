using UnityEngine;

/**
 * Miracle that does massive damage to enemies and stuns them
 *@ author Manny Kwong 
 */

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
               
                if (destroyable.gameObject.layer == 6 || destroyable.gameObject.layer == 12)
                {
                    destroyable.Damage(MiracleEffect);
                    //destroyable.Shake(0.5f,3);
                }
            }
        }
    }
}
