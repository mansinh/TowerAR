using UnityEngine;

/**
 * Miracle that grows trees and fields
 * Puts out fires
 *@ author Manny Kwong 
 */

public class MiracleWater : Miracle
{
    [SerializeField] float _coolDown = 0.2f;
    float _timeSinceAttack = 0;
    [SerializeField] float extinguishSpeed = 0.25f;
    [SerializeField] float growSpeed = 5;
    [SerializeField] float tileHealAmount = 0.5f;

    protected override void OnUpdate()
    {
        _timeSinceAttack += Time.deltaTime;
        if (_timeSinceAttack > _coolDown)
        {
            _timeSinceAttack = 0;
            Collider[] detected = Physics.OverlapSphere(transform.position, Collider.radius);
            foreach (Collider other in detected)
            {
                //Put out fire if other collider is fire
                MiracleFire fire = other.GetComponent<MiracleFire>();
                if (fire != null)
                {
                    fire.OnWater(extinguishSpeed*_coolDown);
                }

                //Grow trees if other collider is a Tree
                Tree tree = other.GetComponent<Tree>();
                if (tree != null)
                {
                    tree.Grow(growSpeed*_coolDown);
                }

                //Grow field if other collider if field
                Field field = other.GetComponent<Field>();
                if (field != null)
                {
                    field.Grow(growSpeed * _coolDown);
                }

                Tile tile = other.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.OnMiracleRain(tileHealAmount);
                }

               
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
}
