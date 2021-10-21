using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleWater : Miracle
{
    [SerializeField] float _coolDown = 0.2f;
    float _timeSinceAttack = 0;
    [SerializeField] float _extinguishSpeed = 0.25f;
    [SerializeField] float _growSpeed = 5;
    protected override void OnHit(RaycastHit hit)
    {
        /*
        base.OnHit(hit);
        Forest forest = hit.collider.GetComponent<Forest>();
        if (forest != null) {
         
            forest.Grow(_growSpeed);
        }

        Tree tree = hit.collider.GetComponent<Tree>();
        if (tree != null)
        {
            tree.GetForest().Grow(_growSpeed);
        }

        Field field = hit.collider.GetComponent<Field>();
        if (field != null)
        {
            field.Grow(_growSpeed);
        }*/
    }

    protected override void OnUpdate()
    {
        _timeSinceAttack += Time.deltaTime;
        if (_timeSinceAttack > _coolDown)
        {
            _timeSinceAttack = 0;
            Collider[] detected = Physics.OverlapSphere(transform.position, Collider.bounds.extents.x);
            foreach (Collider other in detected)
            {
                MiracleFire fire = other.GetComponent<MiracleFire>();
                if (fire != null)
                {
                    fire.OnWater(_extinguishSpeed*_coolDown);
                }

                Forest forest = other.GetComponent<Forest>();
                if (forest != null)
                {

                    forest.Grow(_growSpeed*_coolDown);
                }

                Field field = other.GetComponent<Field>();
                if (field != null)
                {
                    field.Grow(_growSpeed * _coolDown);
                }
            }
        }
    }
}
