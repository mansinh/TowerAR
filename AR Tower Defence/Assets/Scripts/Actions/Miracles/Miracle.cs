using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
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
        Cast();
    }

    public void SetProperties(Damage attackDamage, float lifetime)
    {
        _attackDamage = attackDamage;
        _lifetime = lifetime;
    }

    public void Cast()
    {
        if (MyCursor.instance.GetCursorHitting())
        {
            RaycastHit hit = MyCursor.instance.GetCursorHit();
            transform.position = hit.point;          
            PlayEffects();
            _collider.enabled = true;
        }
    }

    void PlayEffects()
    {
        _visualEffect.Play();
        _soundEffect.Play();
    }

    private void Update()
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
        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            Affect(destroyable);
        }
    }

    protected virtual void Affect(Destroyable destroyable)
    {
        destroyable.Damage(_attackDamage);
    }
}
