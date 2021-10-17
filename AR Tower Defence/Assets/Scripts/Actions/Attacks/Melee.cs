using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Melee :Attack
{

    
    [SerializeField] GameObject _attackImpact;
    SphereCollider meleeCollider;

    private void Awake()
    {
        meleeCollider = GetComponent<SphereCollider>();
        meleeCollider.enabled = false;
    }

    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        meleeCollider.enabled = true;
    }

    
    public override void EndAction()
    {
        
        meleeCollider.enabled = false;
        base.EndAction();
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
