using UnityEngine;

/**
 * Places a building at cursor location when activated
 * @author Manny Kwong
 */

public class BuildingCard : Card
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] Transform[] groundChecks;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] Material mat_placeable;
    [SerializeField] Material mat_notPlaceable;
    [SerializeField] float gridSize = 0.05f;
    [SerializeField] bool ignoreWalls = false;
    [SerializeField] bool ignoreTowers = false;

    Vector3 ghostDisp = new Vector3(0, 0.01f, 0);

    private bool isPlaceable = false;

    protected override void Init()
    {
        SetMaterial(mat_notPlaceable);
        base.Init();
    }

    //Show a placeholder/ghost of the building to be placed at cursor location
    protected override void UpdateGhost(RaycastHit hit)
    {
        Vector3 position = hit.point;
        Transform hitTransform = hit.collider.transform;
        if (hitTransform.GetComponent<Tile>())
        {
            position.y = hitTransform.GetComponent<Tile>().GetTop().y;
        }
        else
        {
            position.y = hitTransform.position.y;
        }
        position = SnapTo(hitTransform.position, position);

        Ghost.transform.position = position + ghostDisp;

        if (CheckForGround() && GetIsUsable())
        {
            if (!isPlaceable)
            {
                SetMaterial(mat_placeable);
                isPlaceable = true;
            }
        }
        else
        {
            if (isPlaceable)
            {
                SetMaterial(mat_notPlaceable);
                isPlaceable = false;
            }
        }
    }

    void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = material;
        }
    }

    
    public Vector3 SnapTo(Vector3 snapFrom, Vector3 position)
    {
        Vector3 disp = position - snapFrom;
        disp.x = Mathf.RoundToInt(disp.x / gridSize) * gridSize;
        
        disp.z = Mathf.RoundToInt(disp.z / gridSize) * gridSize;

        return snapFrom + disp;
    }

    public void SetRotation(float rotation)
    {
        print("dsfsdff" + rotation);
        Ghost.transform.localEulerAngles = new Vector3(0, rotation, 0);
    }

    public bool CheckForGround()
    {
        float height = 0;
        for (int i = 0; i < groundChecks.Length; i++)
        {
            Transform groundCheck = groundChecks[i];
            RaycastHit hit;

            if (Physics.Raycast(groundCheck.position, Vector3.down, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                Tower tower = hit.collider.GetComponent<Tower>();
                Wall wall = hit.collider.GetComponent<Wall>();


                if (tile == null)
                {
                    if (!((wall != null && ignoreWalls) || (tower != null && ignoreTowers)))
                    {
                        return false;
                    }
                }
                else
                {
                    if (tile.GetState() != 100)
                    {
                        return false;
                    }
                    if (i == 0)
                    {
                        height = tile.GetHeight();
                    }
                    else if (height != tile.GetHeight())
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    //Spawn a building in place of the ghost and then discard
    public override bool ActivateCard()
    {
        if (isPlaceable)
        {
            GameObject building = Instantiate(buildingPrefab, World.Instance.transform);
            building.transform.position = Ghost.transform.position - ghostDisp;
            building.transform.localScale = Ghost.transform.localScale;
            building.transform.localEulerAngles = Ghost.transform.localEulerAngles;
            if (building.GetComponent<VillageBuilding>())
            {
                building.GetComponent<VillageBuilding>().IsBuilt = false;
                building.GetComponent<VillageBuilding>().SetHealthPercentage(0.1f);
            }
            GameController.Instance.SelectObject(building.GetComponent<ISelectable>());
            Discard();
            return true;
        }
        return false;
    }
}
