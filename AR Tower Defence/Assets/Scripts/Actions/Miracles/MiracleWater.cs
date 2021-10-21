using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleWater : Miracle
{
    [SerializeField] float _growSpeed;
    protected override void OnHit(RaycastHit hit)
    {
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
        }
    }
}
