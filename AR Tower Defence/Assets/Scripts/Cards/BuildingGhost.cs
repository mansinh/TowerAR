using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] Transform[] groundChecks;
    [SerializeField] MeshRenderer[] meshes;
    [SerializeField] Material ghostMaterial;
    private Vector3 lastValidPosition;

    private void Awake()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = ghostMaterial;
        }
        lastValidPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void MoveTo(Vector3 position)
    {
        transform.position = position;
        if (!CheckForGround())
        {
            transform.position = lastValidPosition;
        }
        else
        {
            lastValidPosition = position;
        }
    }

    public bool CheckForGround()
    {
        float height = 0;

        for(int i = 0; i < groundChecks.Length;i++)
        {
            Transform groundCheck = groundChecks[i];        
            RaycastHit hit;
            if (Physics.Raycast(groundCheck.position, Vector3.down, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile)
                {
                    if (i == 0)
                    {
                        height = tile.GetHeight();
                    }
                    else if (height != tile.GetHeight())
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
