using UnityEngine;

public class BuildingCard : Card
{
    [SerializeField] GameObject _building;

    Tile _targetTile = null;

    protected override void UpdateGhost(RaycastHit hit)
    {
        _targetTile = hit.collider.gameObject.GetComponent<Tile>();

        if (_targetTile)
        {
            Ghost.SetActive(true);
            Ghost.transform.position = _targetTile.GetTop();
            Ghost.transform.localRotation = Quaternion.identity;
        }
        else {
            Ghost.SetActive(false);
        }
    }
    protected override void ActivateCard()
    {
        if (_targetTile)
        {
            GameObject building = Instantiate(_building, WorldRoot.instance.transform);
            building.transform.position = _targetTile.GetTop();
            building.transform.localRotation = Quaternion.identity;
            Deck.RemoveCard(this);
        }
    }
}
