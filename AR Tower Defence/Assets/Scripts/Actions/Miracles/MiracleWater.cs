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
    [SerializeField] float _extinguishSpeed = 0.25f;
    [SerializeField] float _growSpeed = 5;

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
                    fire.OnWater(_extinguishSpeed*_coolDown);
                }

                //Grow trees if other collider is forest
                Forest forest = other.GetComponent<Forest>();
                if (forest != null)
                {

                    forest.Grow(_growSpeed*_coolDown);
                }

                //Grow field if other collider if field
                Field field = other.GetComponent<Field>();
                if (field != null)
                {
                    field.Grow(_growSpeed * _coolDown);
                }
            }
        }
    }
}
