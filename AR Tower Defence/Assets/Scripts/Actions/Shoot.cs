using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Action
{
    Pool projectilePool;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float projectileSpeed;
    [SerializeField] float attackDamage;
    [SerializeField] float criticalDamage;
    [SerializeField] float criticalRate;
    [SerializeField] float stunDamage;
    [SerializeField] float stunRate;
    [SerializeField] float stunDuration;
    [SerializeField] float projectileLifetime;
    

    protected override void Act(Vector3 targetPosition)
    {
        ShootProjectile(targetPosition);
    }

    void ShootProjectile(Vector3 targetPosition) {
        Projectile projectile = projectilePool.GetNextPoolable() as Projectile;
        if (projectile)
        {
            Vector3 projectileDirection = (targetPosition - transform.position).normalized;

            //accuracy
            //speedvariation
            //stun
            float stunActualDuration = 0;
            float additionalDamage = 0;
            if(Random.value < stunRate)
            {
                additionalDamage = stunDamage;
                stunActualDuration = stunDuration;
            }

            if(Random.value < criticalRate){
                projectile.SetProperties((attackDamage + additionalDamage)*criticalDamage, stunActualDuration, projectileLifetime, projectileSpeed, projectileDirection, transform.position);
                Debug.Log("Attack DMG: " + (attackDamage + additionalDamage) * criticalDamage + ", Stun duration: " + stunActualDuration);
            }
            else{
                projectile.SetProperties((attackDamage + additionalDamage), stunActualDuration, projectileLifetime, projectileSpeed, projectileDirection, transform.position);
                Debug.Log("Attack DMG: " + (attackDamage + additionalDamage) + ", Stun duration: " + stunActualDuration);
            }
            projectilePool.Push();
        }
    }

    protected override void Init()
    {
        projectilePool = gameObject.AddComponent<Pool>();
        projectilePool.SetPrefab(projectilePrefab);
    }
}
