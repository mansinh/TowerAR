using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using World;
public class MiracleFire : Miracle
{
    [SerializeField] float _startSize = 0.2f;
    [SerializeField] float _coolDown = 1f;
    [SerializeField] ParticleSystem _burntArea;

    float _timeSinceAttack = 1;

    public override void Activate()
    {

        GetComponent<NavMeshObstacle>().enabled = false;
        VisualEffect.startSize = _startSize;
        base.Activate();

    }

    protected override void OnUpdate()
    {

        _timeSinceAttack += Time.deltaTime;
        if (Lifetime - Life > 1)
        {
            GetComponent<NavMeshObstacle>().enabled = true;
        }
        VisualEffect.startSize = _startSize * Mathf.Min(4 * Life / Lifetime, 1);

        if (_timeSinceAttack > _coolDown)
        {
            _timeSinceAttack = 0;
            Collider[] detected = Physics.OverlapSphere(transform.position, Collider.radius);
            foreach (Collider other in detected)
            {
                Destroyable otherDestroyable = other.GetComponent<Destroyable>();
                if (otherDestroyable)
                {
                    if (!otherDestroyable.IsDestroyed)
                    {
                        otherDestroyable.Damage(MiracleEffect);
                    }
                }

                if (_burntArea)
                {
                    Tile tile = other.GetComponent<Tile>();
                    if (tile)
                    {
                        if (!_burntArea.gameObject.active)
                        {
                            _burntArea.gameObject.SetActive(true);
                            if (!_burntArea.isPlaying)
                            {
                                _burntArea.Play();
                            }
                        }
                        _burntArea.transform.localScale = Vector3.one * Mathf.Min(4 * Life / Lifetime, 1);
                    }
                    else
                    {
                        _burntArea.gameObject.SetActive(false);
                    }
                }
            }
        }
    }


    public void OnWater(float effect)
    {
        Life -= effect;
    }
    protected override void OnLifeOver()
    {
        gameObject.SetActive(false);
    }
}
