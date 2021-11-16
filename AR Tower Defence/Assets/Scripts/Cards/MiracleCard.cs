using UnityEngine;

/**
 * Casts a miracle from miracle controller at cursor location when activated
 * @author Manny Kwong
 */

public class MiracleCard : Card
{
    private MiracleController _miracleController; //Controls miracle activation rate, lifetime, pooling
    private int _maxTimesActivated = 3;
    private int _timesActivated = 0;

    [SerializeField] Light showCursorLight;

    public void SetMaxTimesActivated(int maxTimesActivated)
    {
        _maxTimesActivated = maxTimesActivated;
    }

    public void SetMiracleController(MiracleController miracleController)
    {
        _miracleController = miracleController;
    }

    protected override void UpdateGhost(RaycastHit hit)
    {
        Ghost.transform.position = hit.point;
        if (GetIsUsable())
        {
            showCursorLight.intensity = 0.01f;
        }
        else
        {
            showCursorLight.intensity = 0.002f;
        }
    }

    public override bool ActivateCard()
    {
        //Activate miracle from pool at target position and update activation count
        if (_miracleController.Activate(transform.position))
        {
            _timesActivated++;
            SetGameInfo();
        }

        //Discard card when used up
        if (_timesActivated >= _maxTimesActivated)
        {
            DeactivateCard();
            Discard();
            return true;
        }
        return false;
    }

    //Show card description and show number of unused miracles left
    public override void SetGameInfo()
    {
        GameInfo.Instance.SetSelectedText(_miracleController.GetInfo() + " " + (_maxTimesActivated - _timesActivated) + "/" + _maxTimesActivated);
    }
}
