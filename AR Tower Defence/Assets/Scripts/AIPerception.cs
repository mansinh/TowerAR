using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPerception : MonoBehaviour
{
    Transform detectFrom;
    float detectRange;


    public Collider getClosestTarget(string targetTag)
    {
        Collider closestTarget = null;
        float closestDistance = 10000000000;
        Collider[] detected = Physics.OverlapSphere(detectFrom.position, detectRange, LayerMask.GetMask(targetTag));
        foreach (Collider other in detected)
        {
            RaycastHit hit;
            if (hasLineOfSight(other, detectFrom, out hit))
            {
                if (hit.distance < closestDistance)
                {
                    closestTarget = other;
                    closestDistance = hit.distance;
                }
            }
        }

        return closestTarget;
    }



    bool hasLineOfSight(Collider other, Transform detectFrom, out RaycastHit hit)
    {


        Vector3 direction = (other.transform.position - detectFrom.position+Vector3.up/2).normalized;

        Physics.Raycast(detectFrom.position, direction, out hit);
        if (hit.collider)
        {
            //print(gameObject.name+  " other " + other.name + " hit " + hit.collider.name);
            return hit.collider.gameObject == other.gameObject;
        }
        return false;
    }

    public void setDetectFrom(Transform detectFrom)
    {
        this.detectFrom = detectFrom;
    }
    public void setDetectRange(float detectRange)
    {
        this.detectRange = detectRange;
    }


}
