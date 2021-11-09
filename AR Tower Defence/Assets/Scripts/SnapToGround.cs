using UnityEngine;

[ExecuteInEditMode]
public class SnapToGround : MonoBehaviour
{
    [SerializeField] bool snapToGrid = true;
    [SerializeField] float gridSize = 0.25f;

    World world;
    private void Awake()
    {
        world = FindObjectOfType<World>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            enabled = false;
        }
        if (world)
        {
            Tile tile = world.GetTile(transform.position);
            if (tile)
            {
                Vector3 p = transform.position;
                p.y = tile.GetTop().y;
                transform.position = p;
                if (snapToGrid)
                {
                    transform.position = SnapToGrid(tile.transform.position, transform.position);
                }
            }
        }
    }

    public Vector3 SnapToGrid(Vector3 snapFrom, Vector3 position)
    {
        Vector3 disp = position - snapFrom;
        disp.x = Mathf.RoundToInt(disp.x / gridSize) * gridSize;

        disp.z = Mathf.RoundToInt(disp.z / gridSize) * gridSize;

        return snapFrom + disp;
    }
}
