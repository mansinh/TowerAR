using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Tap to purchase miracle card and draw into hand
 * @author Manny Kwong
 */

[RequireComponent(typeof(MiracleController))]
public class MiracleDeck : CardDeck
{
    [SerializeField] int _maxTimesActivated;
    MiracleController _miracleController;
    [SerializeField] AudioSource activationSound;

    private void Awake()
    {
        _miracleController = GetComponent < MiracleController >();
    }
    protected override Card DrawRandomCard()
    {
        if (activationSound)
        {
            activationSound.Play();
        }
        MiracleCard card = base.DrawRandomCard() as MiracleCard;
        if (card)
        {
            card.SetMiracleController(_miracleController);
            card.SetMaxTimesActivated(_maxTimesActivated);
            card.Select();
        }
        return card;
    }
}
