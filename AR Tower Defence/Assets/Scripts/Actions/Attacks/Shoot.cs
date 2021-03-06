using UnityEngine;

/**
 * Set projectile stats and shoot at a target
 *@ author Manny Kwong 
 */

public class Shoot : Attack
{
    Pool projectilePool;
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _projectileLifetime;
    [SerializeField] int _projectileCount;
    

    protected override void Init()
    {
        base.Init();
        projectilePool = gameObject.AddComponent<Pool>();
        projectilePool.SetPrefab(_projectilePrefab.gameObject);
        projectilePool.SetPoolSize(_projectileCount);
       
    }
    private void Start()
    {
        projectilePool.Init();
    }
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
                projectile.SetProperties(CalulateDamage(), _projectileLifetime, _projectileSpeed, projectileDirection, transform.position);
                projectilePool.Push();
            }
        }
    }    
}
