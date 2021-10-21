using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class Miracle : MonoBehaviour
{
    [SerializeField] protected ParticleSystem VisualEffect;
    [SerializeField] protected AudioSource SoundEffect;
    [SerializeField]  protected Damage MiracleEffect;
    [SerializeField]  protected float Lifetime;
    [SerializeField]  protected float Life = 0f;
    protected Collider Collider;

    private void Awake()
    {
        VisualEffect = GetComponent<ParticleSystem>();
        SoundEffect = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        Collider = GetComponent<Collider>();
        Activate();
    }

    public void SetProperties(Damage attackDamage, float lifetime)
    {
        MiracleEffect = attackDamage;
        Lifetime = lifetime;
    }

    public virtual void Activate()
    {
        Life = Lifetime;
        if (MyCursor.Instance.GetCursorHitting())
        {
            RaycastHit hit = MyCursor.Instance.GetCursorHit();
            OnHit(hit);
            transform.position = hit.point;        
        }
        PlayEffects();
        Collider.enabled = true;

    }

    void PlayEffects()
    {

        VisualEffect.enableEmission = true;
        VisualEffect.Play();
        SoundEffect.Play();
    }

    private void Update()
    {
        OnUpdate();
        LifetimeCounter();     
        if(MyCursor.Instance.GetCursorHitting())
        {
            RaycastHit hit = MyCursor.Instance.GetCursorHit();
            OnHit(hit);
        }  
    }

    protected virtual void OnUpdate() { }

    void LifetimeCounter()
    {
        Life -= Time.deltaTime;
        if (Life < 0)
        {
            OnLifeOver();
        }
    }
    protected virtual void OnLifeOver()
    {
        if (VisualEffect.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
  
    private void OnTriggerEnter(Collider other)
    {
        OnEnterArea(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnStayArea(other);
    }

    protected virtual void OnEnterArea(Collider other)
    {
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            destroyable.Damage(MiracleEffect);
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
