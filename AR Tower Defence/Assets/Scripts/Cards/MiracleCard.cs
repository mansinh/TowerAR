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

    public override bool ActivateCard()
    {
        if (_miracleController.Activate(transform.position))
        {
            _timesActivated++;
            SetGameInfo();
        }
        if (_timesActivated >= _maxTimesActivated)
        {
            DeactivateCard();
            Remove();
            return true;
        }
        return false;
    }

    protected override void SetGameInfo()
    {
        GameInfo.Instance.SetText(Description + " " + (_maxTimesActivated - _timesActivated) +"/" + _maxTimesActivated);
    }
}
