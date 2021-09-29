using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningController : MonoBehaviour
{
    [SerializeField]ParticleSystem lightningEffect;
    [SerializeField] float attackDamage = 3;
    [SerializeField] float attackRange = 0.5f;
    ParticleSystem[] lightningPool = new ParticleSystem[10];
    ParticleSystem crater;
    AudioSource soundEffect;
    int currentIndex = 0;
    CursorController inputController;


    void Start()
    {
        inputController = FindObjectOfType<CursorController>();
        for (int i = 0; i < 10; i++)
        {
            lightningPool[i] = Instantiate(lightningEffect.gameObject,transform).GetComponent<ParticleSystem>();
        }
        soundEffect = GetComponent<AudioSource>();
    }

   
    public void Cast()
    {
        if (inputController.GetCursorHitting())
        {
            RaycastHit hit = inputController.GetCursorHit();
            ParticleSystem lightning = lightningPool[currentIndex];



            string hitTag = hit.collider.tag;
            switch (hitTag)
            {
                case "Enemy":
                    Transform enemyTransform = hit.collider.transform;
                    lightning.transform.forward = enemyTransform.up;
                    lightning.transform.position = enemyTransform.position + enemyTransform.up * 0.1f;
                    break;
                default:
                    lightning.transform.forward = hit.normal;
                    lightning.transform.position = hit.point + hit.normal * 0.1f;
                    break;
            }
            lightning.Play();
            soundEffect.Play();

            DamageInRange(hit);

            currentIndex++;
            if (currentIndex >= 10)
            {
                currentIndex = 0;
            }
        }
    }

    void DamageInRange(RaycastHit hit) {
        Collider[] detected = Physics.OverlapSphere(hit.point, attackRange);

        foreach (Collider other in detected)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.Damage(attackDamage);
                }
            }
        }
    }

  
}
