using UnityEngine;

/**
 * Miracle that restores the health of non enemy destroyables 
 * Also restores corrupt tiles
 *@ author Manny Kwong 
 */

public class MiracleHeal : Miracle
{
    public override void Activate()
    {
        base.Activate(); 
        Collider[] detected = Physics.OverlapSphere(transform.position, Collider.radius);
        foreach (Collider other in detected)
        {
            print("Heal " + other.name);
            
        }
    }
}
