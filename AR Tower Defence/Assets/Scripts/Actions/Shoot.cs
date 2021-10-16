using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Attack
{
    Pool projectilePool;
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _projectileLifetime;
    [SerializeField] int _projectileCount;


    protected override void Act(Vector3 targetPosition)
    {
        base.Act(targetPosition);
        ShootProjectile(targetPosition);
    }

    void ShootProjectile(Vector3 targetPosition) {
        if (projectilePool.GetInactive().Count > 0)
        {
            Projectile projectile = projectilePool.GetNextPoolable().GetComponent<Projectile>();
            if (projectile)
            {
                Vector3 projectileDirection = (targetPosition - transform.position).normalized;

                //accuracy
                //speedvariation

                projectile.SetProperties(CalulateDamage(), _projectileLifetime, _projectileSpeed, projectileDirection, transform.position);
                projectilePool.Push();
            }
        }
    }

    protected override void Init()
    {
        projectilePool = gameObject.AddComponent<Pool>();
        projectilePool.SetPrefab(_projectilePrefab.gameObject);
        projectilePool.SetPoolSize(_projectileCount);
        projectilePool.Init();
    }
}
