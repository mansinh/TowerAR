using UnityEngine;

public class HealEffect : MonoBehaviour
{
    private ParticleSystem _visualEffect;
   

    // Start is called before the first frame update
    void Start()
    {
        _visualEffect = GetComponent<ParticleSystem>();   
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

            }
        }
    }
}
