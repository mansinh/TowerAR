using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Poolable
{
    private Rigidbody _rbody;
    private float _attackDamage;
    private float _stun;
    private float _speed;
    private float _lifetime;
    private bool _hasHit;
    private Vector3 _direction;

    public void SetProperties(float attackDamage, float stun, float lifetime, float speed, Vector3 direction, Vector3 shootFrom) {
        _attackDamage = attackDamage;
        _stun = stun;
        _lifetime = lifetime;
        _speed = speed;
        _direction = direction;
        transform.position = shootFrom;
    }

    public override void Init(Pool pool)
    {
        base.Init(pool);
        _rbody = GetComponent<Rigidbody>();
    }

    IEnumerator LifeTimer() {
        yield return new WaitForSeconds(_lifetime);
        if (!_hasHit)
        {
            OnRelease();
        }
    }

    public override void OnPush()
    {
        _hasHit = false;
        transform.right = _direction;
        _rbody.velocity = _speed * _direction;
        base.OnPush();
    }
    public override void OnRelease()
    {
        base.OnRelease();
    }
    private void OnTriggerEnter(Collider other)
    {
        _hasHit = true;
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            destroyable.Damage(new Damage(_attackDamage, _stun));
        }
        OnRelease();
    }
   
}
