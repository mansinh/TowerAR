using UnityEngine;

public class HealEffect : MonoBehaviour
{
    private ParticleSystem _visualEffect;
    private AudioSource _soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        _visualEffect = GetComponent<ParticleSystem>();
        _soundEffect = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffects()
    {
        if (_visualEffect)
        {
            if (!_visualEffect.isPlaying)
            {
                _visualEffect.enableEmission = true;
                _visualEffect.Play();
                _soundEffect.Play();
            }
        }
    }
}
