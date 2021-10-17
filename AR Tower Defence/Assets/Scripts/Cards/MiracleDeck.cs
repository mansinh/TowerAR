using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MiracleController))]
public class MiracleDeck : CardDeck
{
    [SerializeField] int _maxTimesActivated;
    MiracleController _miracleController;
    private void Awake()
    {
        _miracleController = GetComponent < MiracleController >();
    }
    protected override Card DrawRandomCard()
    {
        MiracleCard card = base.DrawRandomCard() as MiracleCard;
        if (card)
        {
            card.SetMiracleController(_miracleController);
            card.SetMaxTimesActivated(_maxTimesActivated);
        }
        return card;
    }
}
