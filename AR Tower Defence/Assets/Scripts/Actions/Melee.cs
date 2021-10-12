using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Melee :Attack
{

    [SerializeField] protected float _attackRange;
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

        if (Vector3.Distance(targetPosition, transform.position) < _attackRange)
        {            
            meleeCollider.enabled = true;
        }
        
    }

    protected override void EndAction()
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
            print("ATTACK PLAYER HIT "+ CalulateDamage().damage);
            
            destroyable.Damage(CalulateDamage());
            _attackImpact.SetActive(true);
        }
    }


}
