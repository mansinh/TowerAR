using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public float cooldown;
    public float duration;
 

    [SerializeField] public float _timeRemaining = 0;

    [SerializeField] bool isReady = true;
    [SerializeField] bool isActing = false;
    [SerializeField] bool isCooling = false;
    [SerializeField] protected float _range = -1;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        
    }
    
    private void OnEnable() {
        EndAction();
    }


    public bool Activate(Vector3 targetPosition) {
        if (isReady)
        {
            if (Vector3.Distance(targetPosition, transform.position) < _range || _range < 0)
            {             
                Act(targetPosition);
                isActing = true;
                isReady = false;
                isCooling = false;
                _timeRemaining = duration;
                return true;
            }
        }
        return false;
    }


    protected virtual void Act(Vector3 targetPosition)
    {
        isActing = true;
    }

    public virtual void EndAction()
    {
        isActing = false;
        isCooling = true;
        isReady = false;
        _timeRemaining = cooldown;
    }

    public virtual void EndCoolDown() {
        isCooling = false;
        isActing = false;
        isReady = true;
    }

    private void Update()
    {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining < 0)
        {
            if (isCooling)
            {
                EndCoolDown();
            }
            else if (isActing) {
                EndAction();
            }
        }
    }

    
   
}
