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

    public void PlaceUnit(Transform t) {

        Unit unit = Instantiate(unitPrefab);
        unit.transform.position = t.position;
        unit.transform.forward = -t.forward;
        unit.transform.parent = world.transform;
        //world.Refresh();
    }

    
}
