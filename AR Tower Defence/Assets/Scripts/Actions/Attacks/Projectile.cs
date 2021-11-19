using UnityEngine;

/**
 * Move in a direction, damage destroyables on collision or deactivate when lifetime over
 *@ author Manny Kwong 
 */

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody _rbody;
    private Damage _attackDamage;
    private float _speed;
    private float _lifetime;
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

    //Deactivate when life over
    private void Update()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime < 0)
        {
            gameObject.SetActive(false);
        }
    }

    //Start moving when activated
    void OnEnable()
    {
        transform.right = _direction;
        _rbody.velocity = _speed * _direction;
       
    }

    //Damage destroyables on collision
    private void OnTriggerEnter(Collider other)
    {
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            destroyable.Damage(_attackDamage);
        }
        gameObject.SetActive(false);
    }
}
