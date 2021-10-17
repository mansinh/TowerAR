using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleCard : Card
{
    MiracleController _miracleController;
    int _maxTimesActivated = 3;    
    int _timesActivated = 0;

    public void SetMaxTimesActivated(int maxTimesActivated)
    {
        _maxTimesActivated = maxTimesActivated;
    }

    public void SetMiracleController(MiracleController miracleController)
    {
        _miracleController = miracleController;
    }

    protected override void ActivateCard()
    {
        if (_miracleController.Activate(transform.position))
        {
            _timesActivated++;
        }
        if (_timesActivated >= _maxTimesActivated)
        {
            Deck.RemoveCard(this);
        }
    } 
}
