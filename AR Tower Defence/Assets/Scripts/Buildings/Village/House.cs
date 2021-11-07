

using UnityEngine;
/**
* Adds 1 to points rewarded
* TODO add to worshippers, have villagers move around during the day
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
        for (int i = 0; i < occupancy; i++)
        {
            Villager villager = _villagerPool.Push().GetComponent<Villager>();
            villager.Home = this;  
            villager.StartDay();
        }

        /*
        if (FindObjectOfType<Shrine>())
        {
            FindObjectOfType<Shrine>().AddWorshippers(occupancy);
        }*/
    }

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
}

