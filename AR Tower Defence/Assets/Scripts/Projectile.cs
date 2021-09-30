using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rBody;
 
    Unit projectilePool;

    float timeSinceShot;
    [SerializeField] float lifeTime = 5;
    [SerializeField] float attackDamage = 1;
    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
   
    }

    public void Init(Unit owner)
    {
        projectilePool = owner;
    }

    public void Shoot(Vector3 direction, float shootSpeed) {
        gameObject.SetActive(true);
        rBody.velocity = direction * shootSpeed;
        timeSinceShot = 0;
        rBody.useGravity = true;

    }

    private void Update()
    {
        if (rBody.velocity.sqrMagnitude > 0)
        {
            transform.right = rBody.velocity;
        }
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot > lifeTime) {
            ReturnToPool();
        }
    }

    void ReturnToPool() {
        projectilePool.OnProjectileDestroyed(this);
        gameObject.SetActive(false);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DamageEnemy(collision);
        }
        ReturnToPool();
    }

    void DamageEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.Damage(attackDamage);
           
        }
    }
    public void SetAttackDamage(float attackDamage) {
        this.attackDamage = attackDamage; 
    }
}
