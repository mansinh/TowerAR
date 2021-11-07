using UnityEngine;

[ExecuteInEditMode]
public class SnapToGround : MonoBehaviour
{
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
            }
        }
    }
}
