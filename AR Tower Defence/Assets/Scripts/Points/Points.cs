using UnityEngine;
/**
 * Manages the points that can be used to purchase buildings, upgrades and miracles
 * Points can be gained from actions such as slaying enemies, villagers worshipping or harvesting wheat
 * Village buildings augment the amount of points gained from these actions
 * Will need to be balanced 
 *@ author Manny Kwong 
 */
[RequireComponent(typeof(PointsView))]
public class Points : MonoBehaviour
{
    [SerializeField] int totalPoints = 100;
    public static Points Instance;
    private PointsView _view; //Shows points on the screen
    
    private void Awake()
    {
        Instance = this;
        _view = GetComponent<PointsView>();
        _view.SetPoints(totalPoints); 
    }

    //When an enemy is killed award the player with an amount of points depending on the type of enemy and the composition of the village
    //TODO change to enemy types to enum and balance points awarded by each type
    public void EnemyKilled(string enemyType)
    {
        int points = TotalVillagePoints();
        switch (enemyType)
        {
            case "Enemy": { points *= 1; break; }
            case "Fast": { points *= 1; break; }
            case "Flying": { points *= 1; break; }
            case "Tanky": { points *= 1; break; }
        }
        totalPoints += points;
        _view.UpdatePoints(totalPoints);
    }

    public bool PurchaseCardDraw(CardDeck.DeckType deckType)
    {
        //Get the cost of drawing from this type of deck
        int cardDrawCost = 0;
        switch (deckType) {
            case CardDeck.DeckType.Main: { cardDrawCost = 100;  break; }
            case CardDeck.DeckType.Lightning: { cardDrawCost = 20; break; }
            case CardDeck.DeckType.Water: { cardDrawCost = 20; break; }
            case CardDeck.DeckType.Fire: { cardDrawCost = 20; break; }
            case CardDeck.DeckType.Heal: { cardDrawCost = 20; break; }
        }

        //Check if player has enough points, if so go through with the purchase
        if (totalPoints>= cardDrawCost) {
            totalPoints -= cardDrawCost;
            _view.UpdatePoints(totalPoints);
            return true;
        }
        return false;
    }

    //Calculate the points awarded based from the size of the village
    //TODO change to village building types to enum and balance points awarded by each type
    int TotalVillagePoints() {
        int points = GetVillagePoint("Shrine");
        House[] houses = FindObjectsOfType<House>();

        foreach (House h in houses)
        {
            points += GetVillagePoint("House");
        }

        return points;
    }

    int GetVillagePoint(string villageBuildingType) {
        switch (villageBuildingType)
        {
            case "Shrine": { return 10;}
            case "House": { return 1; }
        }
        return 0;
    }

    //Gain some points back when discarding a card
    public void Discarded() {
        totalPoints += 10;
        _view.UpdatePoints(totalPoints);
    }
}
