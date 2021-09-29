using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] Unit unitPrefab;
    World world;
    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceUnit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            string hitTag = hit.collider.tag;
            if (hitTag.Equals("Placeable"))
            {
                Unit unit = Instantiate(unitPrefab);
                unit.transform.position = hit.point;
                unit.transform.forward = hit.normal;
                unit.transform.parent = world.transform;
                //world.Refresh();
            }
        }

    }
}
