using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class Miracle : MonoBehaviour
{
    [SerializeField] ParticleSystem _visualEffect;
    [SerializeField] AudioSource _soundEffect;

 

    private Damage _attackDamage;
    private float _lifetime;
    private Collider _collider;


    private void OnEnable()
    {
        _collider = GetComponent<Collider>();
        Activate();
    }

    public void SetProperties(Damage attackDamage, float lifetime)
    {
        _attackDamage = attackDamage;
        _lifetime = lifetime;
    }

    public virtual void Activate()
    {

        if (MyCursor.Instance.GetCursorHitting())
        {
            RaycastHit hit = MyCursor.Instance.GetCursorHit();
            OnHit(hit);
            transform.position = hit.point;        
        }
        PlayEffects();
        _collider.enabled = true;

    }

   public virtual void Deactivate()
    {
        
    }

    void PlayEffects()
    {

        _visualEffect.enableEmission = true;
        _visualEffect.Play();
        _soundEffect.Play();
    }

    private void Update()
    {
        LifetimeCounter();
       
        if(MyCursor.Instance.GetCursorHitting())
        {
            RaycastHit hit = MyCursor.Instance.GetCursorHit();
            OnHit(hit);
        }
    }

    void LifetimeCounter()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime < 0)
        {
            if (_visualEffect.isStopped)
            {
                gameObject.SetActive(false);
            }
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        //print("enter " + other);
        OnEnterArea(other);
    }

    private void OnTriggerStay(Collider other)
    {
       // print("stay " +other);
        OnStayArea(other);
    }

    protected virtual void OnEnterArea(Collider other)
    {
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            destroyable.Damage(_attackDamage);
        }
    }

    protected virtual void OnStayArea(Collider other)
    {
        
    }

    protected virtual void OnHit(RaycastHit hit)
    {
       // print("hit " + hit.collider.name);
    }
}
