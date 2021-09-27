using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField]ParticleSystem lightningEffect;
    [SerializeField] float baseDamage = 3;

    ParticleSystem[] lightningPool = new ParticleSystem[10];
    ParticleSystem crater;
    AudioSource soundEffect;
    int currentIndex = 0;
    //[SerializeField]AudioClip soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            lightningPool[i] = Instantiate(lightningEffect.gameObject,transform).GetComponent<ParticleSystem>();
        }
        soundEffect = GetComponent<AudioSource>();
    }

   
    public void Cast(RaycastHit hit)
    {
        ParticleSystem lightning = lightningPool[currentIndex];

        string hitTag = hit.collider.tag;

        switch (hitTag)
        {
            case "Enemy":
                Transform enemyTransform = hit.collider.transform;
                lightning.transform.forward = enemyTransform.up;
                lightning.transform.position = enemyTransform.position+ enemyTransform.up*0.1f;
                DamageEnemy(hit);
                break;
            default:
                lightning.transform.forward = hit.normal;
                lightning.transform.position = hit.point + hit.normal * 0.1f;
                break;
        }

            
        lightning.Play();
        soundEffect.Play();
        
        currentIndex++;
        if (currentIndex >= 10) {
            currentIndex = 0;
        }
    }

    void DamageEnemy(RaycastHit hit)
    {
        Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
        if(enemy) {
            enemy.Damage(baseDamage);
        }
    }
}
