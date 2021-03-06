using System.Collections;
using UnityEngine;

/**
 * Drop trees into lumberyard to convert into wood. Spawns building cards when full
 *@ author Manny Kwong 
 */

public class Lumberyard : VillageBuilding
{
    [SerializeField] float maxWood = 500;
    [SerializeField] float wood;
    [SerializeField] Transform timberView;
    [SerializeField] CardDeck mainDeck;
    [SerializeField] GameObject arrow;

    public bool TreeToWood(MyTree tree)
    {
        if (mainDeck.CanDrawCard())
        {
            wood += tree.GetGrowth();
            StartCoroutine(UpdateWood());
            return true;
        }
        return false;
    }

    //Show how much wood there is in the lumberyard by the height of the wood pile
    IEnumerator UpdateWood()
    {
        float woodLevel = Mathf.Min((wood - maxWood) / maxWood *0.2f, 0);

        //Raise the wood pile 
        while (timberView.localPosition.y < woodLevel)
        {
            timberView.localPosition += new Vector3(0, 0.2f / 10, 0);
            yield return new WaitForSeconds(0.1f);
        }

        //Spawn cards when full of wood and reset the height of the wood pile
        if (wood >= maxWood)
        {
            mainDeck.DrawLumberCards();
            wood -= maxWood;
            FinishedBuilding();
            timberView.localPosition = Vector3.down *0.3f;
            StartCoroutine(UpdateWood());
        }
    }

    //Show indicator arrow when a tree is picked up
    public void ShowArrow()
    {
        if (mainDeck.CanDrawCard())
        {
            arrow.SetActive(true);
        }
    }

    public void HideArrow()
    {
        arrow.SetActive(false);
    }

    public override ISelectable GetSelectable()
    {
        return null;
    }

    public override string GetGameInfo(bool showState)
    {
        string description = "LUMBERYARD: Drop trees here for wood.Spawns building cards when full. ";
        if (showState)
        {
            description += wood + "/" + maxWood + " Wood ";
        }
        return description + base.GetGameInfo(showState); ;
    }
}
