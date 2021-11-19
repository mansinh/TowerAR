using UnityEngine;

/**
 * Buildings that attack the closest enemy from a distance by shooting projectiles
 *@ author Manny Kwong 
 */

public class Tower : VillageBuilding
{
    [SerializeField] float _range = 4;
    [SerializeField] float _AiUpdateTime = 0.2f;
    public Attack _attack;
    [SerializeField] protected float maxHeightDiff = 1;
    AIPerception _perception;
    float _timeSinceAIUpdate = 1;

    [SerializeField] Transform _currentTarget;
    [SerializeField] protected string Info;
    [SerializeField] protected AudioClip shootSound;

    private void Start()
    {
        _perception = gameObject.AddComponent<AIPerception>();
        _perception.setDetectFrom(_attack.transform);
        _perception.setDetectRange(_range);
    }

    void Update()
    {
        //Dont do anything if not built
        if (!IsBuilt)
        {
            return;
        }

        //Attack current target if in range
        if (_currentTarget)
        {
            if (_attack.Activate(_currentTarget.position + Vector3.up * _currentTarget.localScale.y / 2))
            {
                if (shootSound)
                {
                    SoundEffects.PlayOneShot(shootSound);
                }
            }
        }

        //Look for closest enemy and set as target 
        _timeSinceAIUpdate += Time.deltaTime;
        if (_timeSinceAIUpdate > _AiUpdateTime)
        {
            Destroyable closestTarget = _perception.getClosestTarget("Enemy", maxHeightDiff);
            if (closestTarget)
            {
                _currentTarget = closestTarget.transform;
            }
            else
            {
                _currentTarget = null;
            }
            _timeSinceAIUpdate = 0;
        }
    }


    public override string GetGameInfo(bool showState)
    {
        return Info + base.GetGameInfo(showState);
    }
}
