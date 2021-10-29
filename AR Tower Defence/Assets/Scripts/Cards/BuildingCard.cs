using UnityEngine;

/**
 * Places a building at cursor location when activated
 * @author Manny Kwong
 */

public class BuildingCard : Card
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private float gridSize = 1f / 3;
    private Tile _targetTile = null;

    private float _rotateTime = 0.8f;
    private float _timeSinceRotate = 0;

    //Show a placeholder/ghost of the building to be placed at cursor location
    protected override void UpdateGhost(RaycastHit hit)
    {
        _targetTile = hit.collider.gameObject.GetComponent<Tile>();

        if (_targetTile)
        {
            Ghost.SetActive(true);
            Vector3 tilePosition = _targetTile.GetTop();

            //Snap postion to a grid within the tile
            float x = Mathf.RoundToInt((hit.point.x - tilePosition.x) / gridSize) * gridSize + tilePosition.x;
            float z = Mathf.RoundToInt((hit.point.z - tilePosition.z) / gridSize) * gridSize + tilePosition.z;
            tilePosition.x = x;
            tilePosition.z = z;
            Ghost.transform.position = tilePosition;
        }
       
        //Rotate ghost 90 degrees periodically
        //TODO may replace with a slider
        _timeSinceRotate += Time.deltaTime;
        if (_timeSinceRotate > _rotateTime) {
            _timeSinceRotate = 0;
            Ghost.transform.localEulerAngles = Ghost.transform.localEulerAngles + Vector3.up * 90;
        }
    }

    
    //Spawn a building in place of the ghost and then discard
    public override bool ActivateCard()
    {
        if (_targetTile)
        {
            GameObject building = Instantiate(buildingPrefab, World.Instance.transform);
            building.transform.position = Ghost.transform.position;
            building.transform.localScale = Ghost.transform.localScale;
            building.transform.localEulerAngles = Ghost.transform.localEulerAngles;
            Discard();
            return true;
        }
        return false;
    }
}
