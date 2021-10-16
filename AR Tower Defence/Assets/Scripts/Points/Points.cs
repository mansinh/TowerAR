using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointsView))]
public class Points : MonoBehaviour
{
    [SerializeField] int _totalPoints = 100;
    [SerializeField] int _initialCardDrawCost = 70;
    public static Points instance;
    PointsView _view;

    Dictionary<string, int> EnemyPoints = new Dictionary<string, int>();

    //*************************************************************************************************************************

    
    private void Awake()
    {
        instance = this;
        _view = GetComponent<PointsView>();
        _view.SetPoints(_totalPoints);
        InitEnemyPoints();
        
    }

   

    //*************************************************************************************************************************

    public void EnemyKilled(string name)
    {
        int pointsAwarded = 0;
        EnemyPoints.TryGetValue(name, out pointsAwarded);
        _totalPoints += pointsAwarded;
        _view.UpdatePoints(_totalPoints);
    }

    public bool PurchaseCardDraw()
    {
        int cardDrawCost = CalculateCardDrawCost();
        if (_totalPoints>= CalculateCardDrawCost()) {
            _totalPoints -= cardDrawCost;
            _view.UpdatePoints(_totalPoints);
            return true;
        }
        return false;
    }

    //*************************************************************************************************************************

    int CalculateCardDrawCost()
    {
        int cardDrawCost = _initialCardDrawCost;
        return cardDrawCost;
    }

    void InitEnemyPoints()
    {
        int basePoints = 10;
        EnemyPoints.Add("Enemy", basePoints);
    }
}
