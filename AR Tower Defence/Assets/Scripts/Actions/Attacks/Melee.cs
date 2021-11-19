using System.Collections;
using UnityEngine;

/**
 * Short range attack
 *@ author Manny Kwong 
 */

[RequireComponent(typeof(SphereCollider))]
public class Melee :Attack
{
    [SerializeField] protected float ActionAnimDelay = 0.1f;

    [SerializeField] GameObject _attackImpact;
    SphereCollider meleeCollider;

 

    protected override void Init()
    {
        base.Init();
        meleeCollider = GetComponent<SphereCollider>();
        meleeCollider.enabled = false;
    }

    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        StartCoroutine(DelayedAction());
    }

    //Delay casting of collider to be in sync with animation
    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(ActionAnimDelay);
        meleeCollider.enabled = true;
    }
    
    public override void EndAction()
    { 
        base.EndAction();
        meleeCollider.enabled = false;
        _attackImpact.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        { 
            destroyable.Damage(CalulateDamage());
            _attackImpact.SetActive(true);
        }
    }


}
