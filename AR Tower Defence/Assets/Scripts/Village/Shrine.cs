

using UnityEngine;
/**
* Represents the player
* Gameover if it dies
* TODO add dancing worshippers, gives points over time during the day
*@ author Manny Kwong 
*/
public class Shrine : VillageBuilding
{
    [SerializeField] MiracleController miracleController;
    [SerializeField] SpriteRenderer symbol;

    protected override void Init()
    {
        base.Init();
        miracleController = GameObject.Find("WaterDeck").GetComponent<MiracleController>();
        miracleController.gameObject.SetActive(false);
    }
  
    protected override void Remove()
    {
        GameController.Instance.GameOver();   
    }

    public override bool Use()
    {
        if (!miracleController.gameObject.active)
        {
            FinishedBuilding();
            if (miracleController)
            {
                miracleController.gameObject.SetActive(true);
                symbol.color = Color.white;
                SetGameInfo();
            }
        }
        return base.Use();
    }

    public override void OnHoverEnter()
    {
        SetGameInfo();
        base.OnHoverEnter();
    }

    public override string GetGameInfo(bool showHealth)
    {
        string description = "SHRINE: Where you live. Your villagers worship here which charges your MP. It is gameover when this is destroyed.";
        if (!miracleController.gameObject.active)
        {
            description += "\nTap to unlock " + miracleController.GetInfo();
        }
        else
        {
            description += miracleController.GetInfo();
        }
        return description + base.GetGameInfo(showHealth);
    }

    public override bool UseImmediately()
    {
        return true;
    }

    public override ISelectable GetSelectable()
    {
        if (!miracleController.gameObject.active)
        {
            return base.GetSelectable();
        }
        return null;
    }
}
