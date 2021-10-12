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

    private void Start()
    {
        _perception = gameObject.AddComponent<AIPerception>();
        _perception.setDetectFrom(transform);
        _perception.setDetectRange(_range);
    }

    void Update()
    {
        _timeSinceAIUpdate += Time.deltaTime;
        if (_timeSinceAIUpdate > _AiUpdateTime)
        {
            Collider closestTarget = _perception.getClosestTarget("Enemy");
            print(closestTarget);
            if (closestTarget)
            {
                Destroyable enemyDestroyable = closestTarget.gameObject.GetComponent<Destroyable>();
                if (enemyDestroyable)
                {
                    Attack(enemyDestroyable);
                }
            }
        }
    }


    void Attack(Destroyable other)
    {
        print("attack");
        //transform.LookAt(other.transform);
        _attack.Activate(other.transform.position+Vector3.up*other.transform.localScale.y/2);
    }
}
