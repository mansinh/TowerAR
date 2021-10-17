using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointsView))]
public class Points : MonoBehaviour
{
    [SerializeField] int _totalPoints = 100;
    public static Points instance;
    PointsView _view;

    Dictionary<string, int> EnemyPoints = new Dictionary<string, int>();

    //*************************************************************************************************************************

    
    private void Awake()
    {
        instance = this;
        _view = GetComponent<PointsView>();
        _view.SetPoints(_totalPoints); 
    }

    //*************************************************************************************************************************

    public void EnemyKilled(string enemyType)
    {
        int pointsAwarded = 10;
        switch (enemyType)
        {
            case "Enemy": { pointsAwarded *=1; break; }        
        }

        _totalPoints += pointsAwarded;
        _view.UpdatePoints(_totalPoints);
    }

    public bool PurchaseCardDraw(string deckType)
    {
        int cardDrawCost = 0;

        switch (deckType) {
            case "Main": { cardDrawCost = 20;  break; }
            case "Lightning": { cardDrawCost = 1; break; }
        }


        if (_totalPoints>= cardDrawCost) {
            _totalPoints -= cardDrawCost;
            _view.UpdatePoints(_totalPoints);
            return true;
        }
        return false;
    }
}
