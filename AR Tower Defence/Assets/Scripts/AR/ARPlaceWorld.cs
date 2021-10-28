
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastHit))]
public class ARPlaceWorld : MonoBehaviour
{
    [SerializeField] World world;
    [SerializeField] GameController gameController;
    [SerializeField]
    ARPlane planePrefab
        ;
    [SerializeField] CanvasGroup noSurfaceFoundText;
    [SerializeField] CanvasGroup ARControlPanel;
    [SerializeField] CanvasGroup startButton;
    [SerializeField] TMP_Text trackingButtonText;


    ARRaycastManager _arRayCastManager;
    ARPlaneManager _planeManager;
    ARSession _arSession;

    static Vector2 screenCenter = new Vector3(Screen.width, Screen.height) / 2;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool IsPlacing = true;
    private float _height = 0;
    private Pose _targetPose;

    private void Awake()
    {
        _arRayCastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        _arSession = FindObjectOfType<ARSession>();

        world.transform.position = new Vector3(0, 100, 0);
        IsPlacing = true;
        trackingButtonText.transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (_arRayCastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            if (IsPlacing)
            {
                _targetPose = hits[0].pose;
                SetWorldHeight();
            }
            noSurfaceFoundText.gameObject.SetActive(false);
            ARControlPanel.gameObject.SetActive(true);
        }
        else
        {
            noSurfaceFoundText.gameObject.SetActive(true);
            ARControlPanel.gameObject.SetActive(false);
        }

    }

    private void SetWorldHeight()
    {
        if (_targetPose != null)
        {
            world.transform.position = _targetPose.position + Vector3.up * _height;
        }
        else
        {
            Vector3 currentPosition = world.transform.position;
            world.transform.position = currentPosition + Vector3.up * _height;
        }
    }

    public void ToggleTracking()
    {
        if (!IsPlacing)
        {
            trackingButtonText.text = "PLACE WORLD";
            Destroy(world.GetComponent<ARAnchor>());
            startButton.interactable = false;

        }
        else
        {
            trackingButtonText.text = "REPOSITION";
            world.gameObject.AddComponent<ARAnchor>();
            world.Refresh();
            startButton.interactable = true;
        }
        SetPlanesActive(!IsPlacing);
        IsPlacing = !IsPlacing;
    }

    void SetPlanesActive(bool isActive)
    {
        _planeManager.enabled = isActive;
        planePrefab.gameObject.SetActive(isActive);
        _planeManager.SetTrackablesActive(isActive);
    }



    public void OnRotateSlider(float value)
    {
        world.transform.eulerAngles = new Vector3(0, value * 360, 0);
    }

    public void OnZoomSlider(float value)
    {
        world.transform.localScale = Vector3.one * value;
    }

    public void OnHeightSlider(float value)
    {
        _height = value;
        SetWorldHeight();
    }

    public void OnStartButton()
    {
        gameController.GameResume();
    }
}
