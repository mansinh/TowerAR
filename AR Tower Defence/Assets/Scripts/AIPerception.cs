using UnityEngine;

/**
 * Detects the closest target within range and in line of sight
 *@ author Manny Kwong 
 */

public class AIPerception : MonoBehaviour
{
    private Transform _detectFrom;
    private float _detectRange;


    public Destroyable getClosestTarget(string targetTag)
    {
        Destroyable closestTarget = null;
        float closestDistance = 10000000000;
        Collider[] detected = Physics.OverlapSphere(_detectFrom.position, _detectRange, LayerMask.GetMask(targetTag));
        foreach (Collider other in detected)
        {
            //Check if collider is destroyable
            Destroyable otherDestroyable = other.GetComponent<Destroyable>();
            if (otherDestroyable)
            {
                //Do not count if destroyable is already destroyed/dead
                if (!otherDestroyable.IsDestroyed)
                {
                    //Check line of sight
                    RaycastHit hit;
                    if (hasLineOfSight(other, _detectFrom, out hit))
                    {
                        //Update closest target
                        if (hit.distance < closestDistance)
                        {
                            closestTarget = otherDestroyable;
                            closestDistance = hit.distance;
                        }
                    }
                }
            }
        }
        return closestTarget;
    }

    bool hasLineOfSight(Collider other, Transform detectFrom, out RaycastHit hit)
    {
        //Cast towards slightly above position of collider
        Vector3 direction = (other.transform.position + Vector3.up / 100 - detectFrom.position).normalized;
        Physics.Raycast(detectFrom.position, direction, out hit);
        if (hit.collider)
        {
            //print(gameObject.name+  " other " + other.name + " hit " + hit.collider.name);
            //If other collider is the first hit by ray then has line of sight
            return hit.collider.gameObject == other.gameObject;
        }
        return false;
    }

    public void setDetectFrom(Transform detectFrom)
    {
        _detectFrom = detectFrom;
    }
    public void setDetectRange(float detectRange)
    {
        _detectRange = detectRange;
    }
}
