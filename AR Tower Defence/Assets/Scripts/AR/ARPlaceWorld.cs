using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastHit))]
public class ARPlaceWorld : MonoBehaviour
{
    WorldRoot worldRoot;
    [SerializeField] Transform ARCursor;
    [SerializeField] Text trackingButtonText;
    ARRaycastManager arRayCastManager;
    ARPlaneManager planeManager;

    static Vector2 screenCenter = new Vector3(Screen.width, Screen.height) / 2;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool isTracking = true;
    Pose targetPose;

    private void Awake()
    {
        arRayCastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        worldRoot = WorldRoot.instance;
        worldRoot.gameObject.SetActive(false);
        trackingButtonText.transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (arRayCastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {  
            targetPose = hits[0].pose;
            ARCursor.transform.position = targetPose.position;
            ARCursor.transform.rotation = targetPose.rotation;          
        }

    }
    public void ToggleTracking() {
        isTracking = !isTracking;
       
        if (isTracking)
        {
            Time.timeScale = 0;
            trackingButtonText.text = "Place World";
            worldRoot.gameObject.SetActive(false);
            Destroy(worldRoot.GetComponent<ARAnchor>());
         
            SetPlanesActive(true);
        }
        else {
            trackingButtonText.text = "Reset";
            Time.timeScale = 1;
            worldRoot.gameObject.SetActive(true);
            worldRoot.gameObject.AddComponent<ARAnchor>();
            worldRoot.transform.position = targetPose.position;
            worldRoot.transform.rotation = targetPose.rotation;
            worldRoot.Refresh();
        
            SetPlanesActive(false);
        }
    }
    void SetPlanesActive(bool isActive) {
        planeManager.enabled = isActive;
        planeManager.SetTrackablesActive(isActive);
    }
}
