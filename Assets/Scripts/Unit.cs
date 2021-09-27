using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float attackDamage = 0.1f;
    [SerializeField] float attackCooldown = 3;
    [SerializeField] int projectilePoolSize = 10;
    [SerializeField] int shootSpeed = 20;
    [SerializeField] float AIupdateTime = 0.2f;

    [SerializeField] Transform shootFrom;
    List<Projectile> projectilePool = new List<Projectile>();
    List<Projectile> activeProjectiles = new List<Projectile>();

    float timeSinceAttack = 0;
    float timeSinceAIUpdate = 0;

    private void Start()
    {
        for (int i = 0; i < projectilePoolSize; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectile.gameObject.SetActive(false);
            projectilePool.Add(projectile);
            projectile.Init(this);
        }
    }
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        timeSinceAIUpdate += Time.deltaTime;


        if (timeSinceAIUpdate > AIupdateTime)
        {
            Collider closestTarget = null;
            float closestDistance = 10000000000;
            Collider[] detected = Physics.OverlapSphere(transform.position, 4);
            timeSinceAIUpdate = 0;
            foreach (Collider other in detected)
            {
                if (other.CompareTag("Enemy"))
                {
                    Vector3 direction = (other.transform.position - shootFrom.position).normalized;
                    RaycastHit hit;
                    Physics.Raycast(shootFrom.position, direction, out hit);

                    if (hit.collider.gameObject == other.gameObject)
                    {
                        if (hit.distance < closestDistance)
                        {
                            closestTarget = other;
                            closestDistance = hit.distance;
                        }
                    }
                }
            }
            if (closestTarget)
            {
                Attack(closestTarget.gameObject.GetComponent<Destroyable>());
            }
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Attack(other.gameObject.GetComponent<Destroyable>());
        }
    }

    void Attack(Destroyable other)
    {

        if (timeSinceAttack > attackCooldown && projectilePool.Count > 0)
        {
            Projectile projectile = projectilePool[0];
            projectile.transform.position = shootFrom.position;
            Vector3 direction = (other.transform.position - shootFrom.position).normalized;

            projectile.Shoot(direction, shootSpeed);
            projectilePool.Remove(projectile);
            activeProjectiles.Add(projectile);
            timeSinceAttack = 0;

        }
    }

    public void OnProjectileDestroyed(Projectile projectile)
    {
        activeProjectiles.Remove(projectile);
        projectilePool.Add(projectile);
    }
}
