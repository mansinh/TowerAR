using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard : Card
{
    [SerializeField] GameObject _Upgrade;
    [SerializeField] protected GameObject UpgradeAnim;
    [SerializeField] protected bool _attackDMG = false;
    [SerializeField] protected bool _critRate = false;
    [SerializeField] protected bool _critDMG = false;
    [SerializeField] protected bool _Slowness = false;
    [SerializeField] protected bool _poison = false;
    [SerializeField] protected bool _stun = false;
    [SerializeField] protected bool _attackSpeed = false;
    Tower _targetTower = null;
    protected override void UpdateGhost(RaycastHit hit)
    {
        _targetTower = hit.collider.gameObject.GetComponent<Tower>();

        if (_targetTower)
        {
            //Upgrade uanimation
            Ghost.SetActive(true);
            Ghost.transform.position = _targetTower.transform.position + Vector3.up * (transform.localScale.y);
            Ghost.transform.localRotation = Quaternion.identity;
        }
        else
        {
            //Disable Upgrade animation
            Ghost.SetActive(false);
        }
    }

    protected override void ActivateCard()
    {
        if (_targetTower)
        {
            if (_attackDMG)
            {
                _targetTower._attack._attackDamageCard += 1;
                Deck.RemoveCard(this);
            }
            if (_critRate)
            {
                _targetTower._attack._critRateCard += 1;
                Deck.RemoveCard(this);
            }
            if (_critDMG)
            {
                _targetTower._attack._critDamageCard += 1;
                Deck.RemoveCard(this);
            }
            if (_Slowness)
            {
                _targetTower._attack._slownessCard += 1;
                Deck.RemoveCard(this);
            }
            if (_poison)
            {
                _targetTower._attack._poisonCard += 1;
                Deck.RemoveCard(this);
            }
            if (_stun)
            {
                _targetTower._attack._stunCard += 1;
                Deck.RemoveCard(this);
            }
            if (_attackSpeed)
            {
                _targetTower._attack._attackSpeedCard += 1;
                Deck.RemoveCard(this);
            }
        }
    }
}
