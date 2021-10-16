using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public float cooldown;
    public float duration;

    public float timeRemaining = 0;
    public float timeStep = 0.1f;

    [SerializeField] bool isReady = true;
    [SerializeField] bool isActing = false;

    private void Awake()
    {
        Init();
        
    }

    private void OnEnable() {
        isReady = true;
        print("Action enabled");
    }

    public void Activate(Vector3 targetPosition) {
        if (isReady)
        {
           
            Act(targetPosition);
            StartCoroutine(ActionTimer());
            isReady = false;         
        }
    }

   

    protected virtual void Init() {}
    protected virtual void Act(Vector3 targetPosition)
    {
        isActing = true;
    }


    public virtual void EndAction()
    {
        isActing = false;
        StartCoroutine(CoolDownTimer());
    }

    IEnumerator ActionTimer()
    {
        timeRemaining = duration;
        for (float i = duration; i > 0; i -= timeStep)
        {
            timeRemaining = i;
            yield return new WaitForSeconds(timeStep);
        }
        EndAction();
       
    }

    IEnumerator CoolDownTimer() {
        timeRemaining = cooldown;
        for (float i = cooldown; i > 0; i -= timeStep)
        {
            timeRemaining = i;
            yield return new WaitForSeconds(timeStep);
        }
        isReady = true;
    }    
}
