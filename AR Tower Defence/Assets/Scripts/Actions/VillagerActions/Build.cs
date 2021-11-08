using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : Attack
{
    [SerializeField] float worshipPoint = 3;
    [SerializeField] GameObject impact;
    protected override void Act(Vector3 targetPosition)
    {
        RaycastHit hit;
        Vector3 direction = targetPosition - transform.position;
        Physics.Raycast(transform.position, direction, out hit);
        if (hit.collider)
        {
            if (hit.collider.GetComponent<VillageBuilding>())
            {
                hit.collider.GetComponent<VillageBuilding>().Damage(CalulateDamage());
                impact.SetActive(true);
            }
        }
    }

    public override void EndAction()
    {


        base.EndAction();
        impact.SetActive(false);
    }
}
