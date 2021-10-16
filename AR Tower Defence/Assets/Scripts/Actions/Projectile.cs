using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody _rbody;
    private Damage _attackDamage;
    private float _stun;
    private float _speed;
    private float _lifetime;
    private bool _hasHit;
    private Vector3 _direction;

    public void SetProperties(Damage attackDamage, float lifetime, float speed, Vector3 direction, Vector3 shootFrom) {
        _attackDamage = attackDamage;    
        _lifetime = lifetime;
        _speed = speed;
        _direction = direction;
        transform.position = shootFrom;
    }

    void Awake() { 
        _rbody = GetComponent<Rigidbody>();
    }

   

    private void Update()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime < 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        _hasHit = false;
        transform.right = _direction;
        _rbody.velocity = _speed * _direction;
       
    }

    

    private void OnTriggerEnter(Collider other)
    {
        _hasHit = true;
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            destroyable.Damage(_attackDamage);
        }
        gameObject.SetActive(false);
    }
   
}
