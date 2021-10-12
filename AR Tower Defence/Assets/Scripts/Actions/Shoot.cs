using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Action
{
    Pool projectilePool;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float projectileSpeed;
    [SerializeField] float attackDamage;
    [SerializeField] float projectileLifetime;

    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        ShootProjectile(targetPosition);
    }

    void ShootProjectile(Vector3 targetPosition) {
        Projectile projectile = projectilePool.GetNextPoolable() as Projectile;
        if (projectile)
        {
            Vector3 projectileDirection = (targetPosition - transform.position).normalized;

            //accuracy
            //speedvariation
            //critical hit
            //stun

            projectile.SetProperties(attackDamage, projectileLifetime, projectileSpeed, projectileDirection, transform.position);
            projectilePool.Push();
        }
    }

    protected override void Init()
    {
        projectilePool = gameObject.AddComponent<Pool>();
        projectilePool.SetPrefab(projectilePrefab);
    }
}
