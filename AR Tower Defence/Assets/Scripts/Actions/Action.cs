using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public float cooldown;
    public float duration;

    public float timeRemaining = 0;
    public float timeStep = 0.1f;

    bool isReady = true;

    private void Awake()
    {
        Init();
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
    protected virtual void Act(Vector3 targetPosition) {}

    IEnumerator ActionTimer()
    {
        timeRemaining = duration;
        for (float i = cooldown; i > 0; i -= timeStep)
        {
            timeRemaining = i;
            yield return new WaitForSeconds(timeStep);
        }
        StartCoroutine(CoolDownTimer());
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
