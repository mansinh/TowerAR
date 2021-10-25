using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;
public class MiracleHeal : Miracle
{
    public override void Activate()
    {
        base.Activate(); 
        Collider[] detected = Physics.OverlapSphere(transform.position, Collider.radius);
        foreach (Collider other in detected)
        {
            print("Heal " + other.name);
            Tile tile = other.GetComponent<Tile>();
            if (tile != null)
            {
               tile.Heal();
            }
            
            World.World.Instance.UpdateView();
            
            Destroyable destroyable = other.GetComponent<Destroyable>();
           
            if (destroyable != null)
            {
                if (destroyable.gameObject.layer != 6)
                {
                    destroyable.Damage(MiracleEffect);
                }
            }
        }
    }
}
