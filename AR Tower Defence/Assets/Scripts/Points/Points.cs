using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointsView))]
public class Points : MonoBehaviour
{
    [SerializeField] int _totalPoints = 100;
    public static Points Instance;
    PointsView _view;

    Dictionary<string, int> EnemyPoints = new Dictionary<string, int>();

    //*************************************************************************************************************************

    
    private void Awake()
    {
        Instance = this;
        _view = GetComponent<PointsView>();
        _view.SetPoints(_totalPoints); 
    }

    //*************************************************************************************************************************

    public void EnemyKilled(string enemyType)
    {
        int points = TotalVillagePoints();
        switch (enemyType)
        {
            case "Enemy": { points *= 1; break; }
        }
        _totalPoints += points;
        _view.UpdatePoints(_totalPoints);
    }

    public bool PurchaseCardDraw(CardDeck.DeckType deckType)
    {
        int cardDrawCost = 0;

        switch (deckType) {
            case CardDeck.DeckType.Main: { cardDrawCost = 100;  break; }
            case CardDeck.DeckType.Lightning: { cardDrawCost = 20; break; }
            case CardDeck.DeckType.Water: { cardDrawCost = 20; break; }
            case CardDeck.DeckType.Fire: { cardDrawCost = 20; break; }
            case CardDeck.DeckType.Heal: { cardDrawCost = 20; break; }
        }

        if (_totalPoints>= cardDrawCost) {
            _totalPoints -= cardDrawCost;
            _view.UpdatePoints(_totalPoints);
            return true;
        }
        return false;
    }

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

    public void RemovedCard() {
        _totalPoints += 10;
        _view.UpdatePoints(_totalPoints);
    }
}
