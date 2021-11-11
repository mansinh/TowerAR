

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
            }
        }
        return base.Use();
    }

    
}
