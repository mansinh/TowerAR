using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager; 
    public GameObject cursor;
    public World world;

    public bool placeWorld = true;
    bool placeable = false;

    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
        world.gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (placeWorld) {
            if (!planeManager.enabled) {
                planeManager.enabled = true;
            }
            UpdateCursor();
            if (Input.touchCount > 0 && placeable) {
                
                world.transform.position = cursor.transform.position;
                world.transform.up = -cursor.transform.forward;
                world.gameObject.SetActive(true);
                world.Init();
                //planeManager.enabled = false;
                placeWorld = false;
            }
           
        }
        
    }

    void UpdateCursor() {
        Vector3 position = Camera.main.ViewportToScreenPoint(new Vector2(0.5f,0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            placeable = true;
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
        else {
            placeable = false;
        }
        
    }
}
