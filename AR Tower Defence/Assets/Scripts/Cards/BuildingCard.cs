using UnityEngine;

public class BuildingCard : Card
{
    [SerializeField] GameObject _building;

    Tile _targetTile = null;

    float _rotateTime = 0.8f;
    float _timeSinceRotate = 0;

    protected override void UpdateGhost(RaycastHit hit)
    {
        _targetTile = hit.collider.gameObject.GetComponent<Tile>();

        if (_targetTile)
        {
            Ghost.SetActive(true);
            Ghost.transform.position = _targetTile.GetTop();
        }
       
        _timeSinceRotate += Time.deltaTime;
        if (_timeSinceRotate > _rotateTime) {
            _timeSinceRotate = 0;
            Ghost.transform.localEulerAngles = Ghost.transform.localEulerAngles + Vector3.up * 90;
        }
    }

    

    protected override void ActivateCard()
    {
        if (_targetTile)
        {
            GameObject building = Instantiate(_building, WorldRoot.Instance.transform);
            building.transform.position = _targetTile.GetTop();
            building.transform.localEulerAngles = Ghost.transform.localEulerAngles;
            Deck.RemoveCard(this);
        }
    }
}
