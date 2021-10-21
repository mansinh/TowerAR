using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MiracleFire : Miracle
{
    [SerializeField] float _startSize = 0.2f;
    [SerializeField] float _coolDown = 1f;

    float _timeSinceAttack = 0;

    public override void Activate()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        VisualEffect.startSize = _startSize;
        base.Activate();
    }

    protected override void OnUpdate()
    {
        _timeSinceAttack += Time.deltaTime;
        if (Lifetime-Life > 1) {
            GetComponent<NavMeshObstacle>().enabled = true;
        }
        VisualEffect.startSize = _startSize * Mathf.Min(4*Life / Lifetime,1);

        if (_timeSinceAttack > _coolDown)
        {
            _timeSinceAttack = 0;
            Collider[] detected = Physics.OverlapSphere(transform.position, Collider.bounds.extents.x);
            foreach (Collider other in detected)
            {
                Destroyable otherDestroyable = other.GetComponent<Destroyable>();
                if (otherDestroyable)
                {
                    if (!otherDestroyable.IsDestroyed)
                    {
                        otherDestroyable.Damage(MiracleEffect);
                    }
                }
            }
        }
    }


    public void OnWater(float effect)
    {
        Life -= effect;
    }
    protected override void OnLifeOver()
    {
        gameObject.SetActive(false);
    }
}
