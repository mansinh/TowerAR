using UnityEngine;
using UnityEngine.AI;

/**
 * Miracle that does small damage over time to all destroyables and blocks enemies 
 *@ author Manny Kwong 
 */

public class MiracleFire : Miracle
{
    [SerializeField] float startSize = 0.2f;
    [SerializeField] float coolDown = 1f;

    float _timeSinceAttack = 1;

    public override void Activate()
    {

        GetComponent<NavMeshObstacle>().enabled = false;
        VisualEffect.startSize = startSize;
        base.Activate();

    }

    protected override void OnUpdate()
    {
        //Damage all destroyables in range over time
        _timeSinceAttack += Time.deltaTime;
        if (Lifetime - Life > 1)
        {
            GetComponent<NavMeshObstacle>().enabled = true;
        }

        //Make flames smaller as it nears the end of its lifetime
        VisualEffect.startSize = startSize * Mathf.Min(4 * Life / Lifetime, 1);

        if (_timeSinceAttack > coolDown)
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
            }
        }
    }

    //Be put out by water
    public void OnWater(float effect)
    {
        Life -= effect;
    }
    protected override void OnLifeOver()
    {
        gameObject.SetActive(false);
    }
}
