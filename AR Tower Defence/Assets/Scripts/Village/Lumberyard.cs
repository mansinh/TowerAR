using System.Collections;
using UnityEngine;

/**
 * Increase chance for building cards or maybe spend points to spawn building cards
 * Yet to be implemented
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
    IEnumerator UpdateWood()
    {
        float woodLevel = Mathf.Min((wood - maxWood) / maxWood / 10,0);
       
        while (timberView.localPosition.y < woodLevel)
        {

            timberView.localPosition += new Vector3(0, 0.15f / 10,0);
            yield return new WaitForSeconds(0.1f);
        }
        if (wood > maxWood)
        {
            mainDeck.DrawLumberCards();
            wood -= maxWood;
            FinishedBuilding();
            timberView.localPosition= Vector3.down/5;
            StartCoroutine(UpdateWood());
        }
    }

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
}
