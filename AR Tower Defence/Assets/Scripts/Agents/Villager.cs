using UnityEngine;

public class Villager : Agent
{
    public House Home;
    private bool _isDay = false;
    private Shrine _shrine;
    [SerializeField] Worship worshipAction;
    public VillagerState State;
    bool isBuilding = false;

    public enum VillagerState
    {
        Building,
        Worshipping,
        Walking,
        Idling,
        Sleeping
    }

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

       

        //Periodically decide on target, not every frame as that may be expensive
        TimeSinceAIUpdate += Time.deltaTime;
        if (TimeSinceAIUpdate > AiUpdateTime)
        {
            if (_isDay)
            {

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
              
                if (_shrine != null && !isBuilding)
                {
                    SetTarget(_shrine.transform, 0.3f);
                }

            }
            else {
                SetTarget(Home.Door.transform,0);
            }
            TimeSinceAIUpdate = 0;
        }

        Vector3 velocity = GetVelocityFraction();
        //Turn towards current target and act if in range and cooldown over
        if (CurrentTarget != null)
        {
            if (_isDay)
            {
                if (isBuilding)
                {
                    if ((CurrentTarget.position - transform.position).sqrMagnitude < 0.25)
                    {
                        State = VillagerState.Building;
                        if (_action != null )
                        {

                            if (_action.Activate(CurrentTarget.position))
                            {
                                transform.LookAt(CurrentTarget.position);
                            }
                            
                        }
                    }
                }
                else if (CurrentTarget.GetComponent<Shrine>())
                {
                    if ((CurrentTarget.position - transform.position).sqrMagnitude < 0.25)
                    {
                        State = VillagerState.Worshipping;
                        if (worshipAction != null)
                        {
                            worshipAction.Activate(CurrentTarget.position);
                      
                               
                         
                        }
                        return;
                    }
                }
            }
            else
            {
                if ((Home.Door.transform.position - transform.position).sqrMagnitude < 0.02)
                {
                    State = VillagerState.Sleeping;
                    Sleep();
                    return;
                }
            }
        }



        if (velocity.sqrMagnitude > 0.01)
        {
            State = VillagerState.Walking;
        }
        else
        {
            State = VillagerState.Idling;
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

}
