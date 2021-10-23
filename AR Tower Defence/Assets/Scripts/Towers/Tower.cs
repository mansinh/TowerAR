using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : VillageBuilding
{
    [SerializeField] float _range = 4;
    [SerializeField] float _AiUpdateTime = 0.2f;
    [SerializeField] public Attack _attack;

    AIPerception _perception;
    float _timeSinceAIUpdate = 1;

    [SerializeField] Transform _currentTarget;

    private void Start()
    {
        _perception = gameObject.AddComponent<AIPerception>();
        _perception.setDetectFrom(_attack.transform);
        _perception.setDetectRange(_range);
    }

    void Update()
    {
        if (_currentTarget)
        {
            if (_attack.Activate(_currentTarget.position + Vector3.up*_currentTarget.localScale.y/2))
            {
                //transform.LookAt(_currentTarget);
            }
        }

        _timeSinceAIUpdate += Time.deltaTime;
        if (_timeSinceAIUpdate > _AiUpdateTime)
        {
            Destroyable closestTarget = _perception.getClosestTarget("Enemy");
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
}
