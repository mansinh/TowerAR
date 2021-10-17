using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] float _range = 4;
    [SerializeField] float _AiUpdateTime = 0.2f;
    [SerializeField] Attack _attack;

    AIPerception _perception;
    float _timeSinceAIUpdate = 1;

    [SerializeField] Transform _currentTarget;

    private void Start()
    {
        _perception = gameObject.AddComponent<AIPerception>();
        _perception.setDetectFrom(transform);
        _perception.setDetectRange(_range);
    }

    void Update()
    {
        if (_currentTarget)
        {
            if (_attack.Activate(_currentTarget.position + Vector3.up * _currentTarget.transform.localScale.y / 2))
            {
                //transform.LookAt(_currentTarget);
            }
        }

        _timeSinceAIUpdate += Time.deltaTime;
        if (_timeSinceAIUpdate > _AiUpdateTime)
        {
            Collider closestTarget = _perception.getClosestTarget("Enemy");     
            if (closestTarget)
            {
                _currentTarget = closestTarget.transform;
            }
            _timeSinceAIUpdate = 0;
        }
    }
}
