using UnityEngine;
/*
* Villagers repair/construct buildings and worship at the shrine for MP during the day and sleep at homes at night
*@ author Manny Kwong 
*/
public class Villager : Agent, IHoverable
{
    public House Home;
    private bool _isDay = false;
    private Shrine _shrine;
    [SerializeField] Worship worshipAction;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite standing, hammerUp, hammerDown, handsUp;

    bool isBuilding = false;

    protected override void Init()
    {
        Name = "Villager";
        TargetName = "Player";
        if (FindObjectOfType<Shrine>())
        {
            _shrine = FindObjectOfType<Shrine>();
        }
        base.Init();
    }

    public void StartDay()
    {
        _isDay = true;
        transform.position = Home.Door.position; 
    }

    public void EndDay()
    {
        _isDay = false;
    }

    void Update()
    {       
        Animate();

        //Periodically decide on target, not every frame as that may be expensive
        TimeSinceAIUpdate += Time.deltaTime;
        if (TimeSinceAIUpdate > AiUpdateTime)
        {
            if (_isDay)
            {
                //Target building that needs repairing/constructing
                VillageBuilding[] buildings = FindObjectsOfType<VillageBuilding>();
                isBuilding = false;
                foreach(VillageBuilding building in buildings)
                {
                    if (building.GetHealthPercentage() < 1)
                    {
                        SetTarget(building.transform, 0.3f);
                        isBuilding = true;
                        
                        break;
                    }
                }
                if (!isBuilding)
                {
                    SoundEffects.Stop();
                }

                //Target the shrine if there are no buildings that need to be repaired/constructed
                if (_shrine != null && !isBuilding)
                {
                    SetTarget(_shrine.transform, 0.5f);
                }

            }
            else {
                SetTarget(Home.Door.transform,0.05f);
            }
            TimeSinceAIUpdate = 0;
        }

        
        //Turn towards current target and act if in range and cooldown over
        if (CurrentTarget != null)
        {
            transform.LookAt(CurrentTarget.position);
            if (_isDay)
            {
                //Use build action when in range of a building that needs repairing/constructing
                if (isBuilding)
                {
                    if ((CurrentTarget.position - transform.position).sqrMagnitude < 0.25)
                    {
                        State = AgentState.Action0;
                        if (_action != null)
                        {
                            if (_action.Activate(CurrentTarget.position))
                            {                               
                                if (!SoundEffects.isPlaying)
                                {
                                    SoundEffects.loop = true;
                                    SoundEffects.PlayOneShot(SoundManager.Instance.SoundClips[(int)SoundManager.SoundType.Build]);
                                }
                            }
                        }
                    }
                    else
                    {
                        SetMoveState();
                    }
                }
                //Use worship action when in range of shrine
                else if (CurrentTarget.GetComponent<Shrine>())
                {
                    if ((CurrentTarget.position - transform.position).sqrMagnitude < 0.25)
                    {
                        State = AgentState.Action1;
                        if (worshipAction != null)
                        {
                            worshipAction.Activate(CurrentTarget.position); 
                        }
                        return;
                    }
                    else
                    {
                        SetMoveState();
                    }
                }
            }
            else
            {
                if ((Home.transform.position - transform.position).sqrMagnitude < 0.1)
                {
                    Sleep();
                    return;
                }
            }
        }
    }

    private void Sleep()
    {
        CurrentTarget = null;
        gameObject.SetActive(false);
    }

    protected override void Remove()
    {
        //Points.Instance.EnemyKilled(Name);
        base.Remove();

    }

    public void OnHoverEnter()
    {
        GameInfo.Instance.SetHoverText("VILLAGER: worships at the shrine to charge MP. Repairs buildings. Returns home to sleep at night.");
    }

    public void OnHoverStay() {}

    public void OnHoverLeave()
    {
        GameInfo.Instance.SetHoverText("");
    }

    public ISelectable GetSelectable()
    {
        return null;
    }

    float timeSinceLastFrame = 0;
    float frameTime = 0.5f;
    void Animate()
    {
        //Flip sprite when moving to the left
        sprite.flipX = Vector3.Dot(transform.forward, GameController.Instance.cameraTransform.right) >= 0;
       
        //Change animation depending on state/action
        switch (State)
        {
            case AgentState.Running: RunAnimation(); break;
            case AgentState.Action0: BuildingAnimation(); break;
            case AgentState.Action1: WorshipAnimation(); break;
            case AgentState.Idling: IdleAnimation(); break;
        }
        timeSinceLastFrame += Time.deltaTime;
    }

    //Set sprite state when moving
    void SetMoveState()
    {
        Vector3 velocity = GetVelocityFraction();
        if (velocity.sqrMagnitude > 0.01)
        {
            State = AgentState.Running;
        }
        else
        {
            State = AgentState.Idling;
        }
    }

    //Bob up and down for makeshift running animation
    void RunAnimation()
    {
        _view.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * 60)/30, 0);
    }

    //Makeshift animation switching between 2 sprites for building
    void BuildingAnimation()
    {
        if (timeSinceLastFrame < frameTime/2)
        {
            sprite.sprite = hammerDown;
        }
        else if (timeSinceLastFrame < frameTime)
        {
            sprite.sprite = hammerUp;
        }
        else
        {
            timeSinceLastFrame = 0;
        }
    }


    //Makeshift animation switching between 2 sprites for worshipping
    void WorshipAnimation()
    {
        print("Worship anim");
        if (timeSinceLastFrame < frameTime)
        {
            print("stand");
            sprite.sprite = standing;
        }
        else if (timeSinceLastFrame <  2*frameTime)
        {
            print("hands up");
            sprite.sprite = handsUp;
        }
        else
        {
            timeSinceLastFrame = 0;
        }
    }

    //Reset sprite when idle
    void IdleAnimation()
    {
        sprite.sprite = standing;
    }
}

