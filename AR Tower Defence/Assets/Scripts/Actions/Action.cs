/**
 * Controls the timing of actions and cooldown
 *@ author Manny Kwong 
 */

using UnityEngine;

public class Action : MonoBehaviour
{
    public float cooldown;
    public float duration;
    public float TimeRemaining = 0;

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
    
    //Reset action on enable
    private void OnEnable() {
        EndAction();
    }

    //Do action if target is within range and cooldown is over
    public bool Activate(Vector3 targetPosition) {
        if (isReady)
        {
            if (Vector3.Distance(targetPosition, transform.position) < _range || _range < 0)
            {             
                Act(targetPosition);
                isActing = true;
                isReady = false;
                isCooling = false;
                TimeRemaining = duration;
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
        TimeRemaining = cooldown;
    }

    public virtual void EndCoolDown() {
        isCooling = false;
        isActing = false;
        isReady = true;
    }

    //Countdown timer
    private void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining < 0)
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
