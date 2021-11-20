
using UnityEngine;

/**
* Adds 1 villager and increases the max points available
*@ author Manny Kwong 
*/

public class House : VillageBuilding
{
    Pool _villagerPool;
    [SerializeField] int occupancy = 1;
    [SerializeField] int maxOccupancy = 3;
    [SerializeField] bool isDay = false;
    public Transform Door;

    [SerializeField] Villager villagerPrefab;

    private void Start()
    {
        _villagerPool = gameObject.AddComponent<Pool>();
        _villagerPool.SetPrefab(villagerPrefab.gameObject);
        _villagerPool.SetPoolSize(maxOccupancy);
        _villagerPool.Init();
        foreach (GameObject o in _villagerPool.Inactive)
        {
            Villager villager = o.GetComponent<Villager>();
            villager.Home = this;
        }
    }

    private void Update()
    {
        if (DayNightController.Instance.isDay)
        {
            if (!isDay)
            {
                StartDay();
            }
        }
        else
        {
            if (isDay)
            {
                EndDay();
            }
        }
    }

    void StartDay()
    {
        isDay = true;
        //if (IsBuilt)
        //{
            WakeVillagers();
        //}
    }

    //Send villagers out during the day
    void WakeVillagers()
    {
        for (int i = 0; i < occupancy; i++)
        {
            GameObject o = _villagerPool.Push();
            if (o)
            {
                Villager villager = o.GetComponent<Villager>();
               
                villager.StartDay();
            }
        }
    }

    public override void FinishedBuilding()
    {
        base.FinishedBuilding();
        WakeVillagers();
    }

    //Recall villagers at night
    void EndDay()
    {
        
        for (int i = 0; i < _villagerPool.Active.Count; i++)
        {
            if (_villagerPool.Active[i].GetComponent<Villager>())
            {
                _villagerPool.Active[i].GetComponent<Villager>().EndDay();
            }
        }
        isDay = false;
        /*
        if (FindObjectOfType<Shrine>())
        {
            FindObjectOfType<Shrine>().RemoveWorshippers(occupancy);
        }*/
    }

    protected override void Remove()
    {
        foreach (GameObject villager in _villagerPool.Active)
        {
            if (villager.GetComponent<Villager>())
            {
                villager.GetComponent<Villager>().TriggerDeath();
            }
        }
        base.Remove();
    }

    public override string GetGameInfo(bool showHealth)
    {
        return "HOUSE: Spawns a villager and increases your maximum MP. " + base.GetGameInfo(showHealth); 
    }
}

