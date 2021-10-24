using UnityEngine;
using World;
public class BuildingCard : Card
{
    [SerializeField] GameObject _building;
    [SerializeField] float _tileSize = 1f / 3;
    Tile _targetTile = null;

    float _rotateTime = 0.8f;
    float _timeSinceRotate = 0;

    protected override void UpdateGhost(RaycastHit hit)
    {
        _targetTile = hit.collider.gameObject.GetComponent<Tile>();

        if (_targetTile)
        {
            Ghost.SetActive(true);
            Vector3 tilePosition = _targetTile.GetTop();

           

            float x = Mathf.RoundToInt((hit.point.x - tilePosition.x) / _tileSize) * _tileSize + tilePosition.x;
            float z = Mathf.RoundToInt((hit.point.z - tilePosition.z) / _tileSize) * _tileSize + tilePosition.z;
            tilePosition.x = x;
            tilePosition.z = z;
            Ghost.transform.position = tilePosition;
        }
       
        _timeSinceRotate += Time.deltaTime;
        if (_timeSinceRotate > _rotateTime) {
            _timeSinceRotate = 0;
            Ghost.transform.localEulerAngles = Ghost.transform.localEulerAngles + Vector3.up * 90;
            Ghost.transform.localScale = new Vector3(1,1+Random.Range(-0.1f,0.1f),1);
        }
    }

    

    protected override void ActivateCard()
    {
        if (_targetTile)
        {
            GameObject building = Instantiate(_building, World.World.Instance.transform);
            building.transform.position = Ghost.transform.position;
            building.transform.localScale = Ghost.transform.localScale;
            building.transform.localEulerAngles = Ghost.transform.localEulerAngles;
            Deck.RemoveCard(this);
        }
    }
}
