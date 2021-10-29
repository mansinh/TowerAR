using UnityEngine;

/**
 * Controls the lifetime, pooling and spawning of miracles
 * @author Manny Kwong
 */

public class MiracleController : Attack
{ 
    Pool _miraclePool;
    [SerializeField] Miracle _miraclePrefab;
    [SerializeField] float _miracleLifetime;
    [SerializeField] int _miracleCount;

    protected override void Init()
    {
        base.Init();
        _miraclePool = gameObject.AddComponent<Pool>();
        _miraclePool.SetPrefab(_miraclePrefab.gameObject);
        _miraclePool.SetPoolSize(_miracleCount);
        
    }

    private void Start()
    {
        _miraclePool.Init();
    }

    //Spawn miracle at cursor location when miracle card is activated (when cooldown is finished and there are miracles left in pool)
    protected override void Act(Vector3 targetPosition)
    {
        if (_miraclePool.GetInactive().Count > 0)
        {
            Miracle miracle = _miraclePool.GetNextPoolable().GetComponent<Miracle>();
            if (miracle)
            {        
                miracle.SetProperties(CalulateDamage(), _miracleLifetime);
                _miraclePool.Push();
            }
        }
    }
}
