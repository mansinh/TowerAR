using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * Controls the placing of the game world onto AR space and the menus for adjusting the position, rotation, zoom and height of the scene
 *@ author Manny Kwong 
 */

[RequireComponent(typeof(ARRaycastHit))]
public class ARController : MonoBehaviour
{
    [SerializeField] private World world; //Game world to be placed
    [SerializeField] private GameController gameController; //Game input controls and menus
    [SerializeField] private ARPlane planePrefab;
    [SerializeField] private CanvasGroup noSurfaceFoundText; 
    [SerializeField] private CanvasGroup ARControlPanel; //Panel containing the sliders for controlling rotation, zoom, and height
    [SerializeField] private CanvasGroup startButton; 
    [SerializeField] private TMP_Text positionWorldButtonText;
    [SerializeField] private Slider zoomSlider;
    [SerializeField] private Slider heightSlider;

    private ARRaycastManager _arRayCastManager;
    private ARPlaneManager _planeManager;
    private ARSession _arSession;

    static Vector2 screenCenter = new Vector3(Screen.width, Screen.height) / 2;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool IsPositioning = true; //Whether or not the game worlds position is being changed or not
    private float _height = 0; //How high game world is above the AR plane its anchored to
    private Pose _targetPose; //Point on AR plane in front of cursor

    private void Awake()
    {
        //Find AR managers
        _arRayCastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        _arSession = FindObjectOfType<ARSession>();

        //Set game world out of sight
        world.transform.position = new Vector3(0, 100, 0);

        IsPositioning = true;
        heightSlider.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Look for a point on AR plane in front of the cursor at the center of the screen
        if (_arRayCastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            //Place world at point of contact if positoning world at a given height
            if (IsPositioning)
            {
                _targetPose = hits[0].pose;
                SetWorldPosition();
            }

            //Turn off no surface found text and turn on the AR control panel
            noSurfaceFoundText.gameObject.SetActive(false);
            ARControlPanel.gameObject.SetActive(true);
        }
        else
        {
            //Tell the player if no AR planes found in front of cursor, and turn off the AR control panel
            noSurfaceFoundText.gameObject.SetActive(true);
            ARControlPanel.gameObject.SetActive(false);
        }

    }

    //Place world on AR plane at the given height. The height is scaled with the amount zoom normalized to the initial scale of the AR space (50)
    private void SetWorldPosition()
    {
        float zoom = transform.localScale.y / 50;
        if (_targetPose != null)
        {
            world.transform.position = _targetPose.position + Vector3.up * _height * zoom;
        }
        else
        {
            Vector3 currentPosition = world.transform.position;
            world.transform.position = currentPosition + Vector3.up * _height * zoom;
        }
    }


    public void OnRotateSlider(float value)
    {
        world.transform.eulerAngles = new Vector3(0, value * 360, 0);
    }

    //Zooms in and out by changing the scale of the AR space centered around the user 
    //If the world is set in position, zooming out will cause the world to drift away from the AR space origin
    public void OnZoomSlider(float value)
    {
        transform.localScale = Vector3.one * value;
    }

    //Increasing the height of the world when it is not set in position in AR space may cause it to move out of view
    //Chasing the world with the camera will cause the world move further away
    public void OnHeightSlider(float value)
    {
        _height = value;
        SetWorldPosition();
    }

    //Toggle whether to change the position of the game world on the horizontal AR plane when the position world button is pressed
    //Allow zooming only when the world is not anchored to prevent drifting
    //Allow adjusting the height of the world only when it is anchored to prevent chasing the world further away
    //Toggling the visiblilty of the zoom and height sliders declutters the control panel
    public void TogglePositioning()
    {
       
        if (!IsPositioning)
        {
            //Look for new position
            positionWorldButtonText.text = "POSITION WORLD";
            startButton.interactable = false;
            heightSlider.interactable = false;
        }
        else
        {
            //World is set in position
            positionWorldButtonText.text = "REPOSITION";
            heightSlider.interactable = true; 
            startButton.interactable = true;
        }
        SetPlanesActive(!IsPositioning);
        IsPositioning = !IsPositioning;
    }

    //Turn the AR planes on when looking for a position for the world and off when the world is set in position 
    void SetPlanesActive(bool isActive)
    {
        //Turn off plane manager when world is set in position to prevent it from looking for and spawning more AR planes
        //Turn on if positioning the world
        _planeManager.enabled = isActive;
        
        //Turn off all existing AR planes when anchored to prevent cluttering the screen and collisions during gameplay
        //Turn on if positioning the world
        planePrefab.gameObject.SetActive(isActive);
        _planeManager.SetTrackablesActive(isActive);
    }

    //When the position of the world is set, the game is ready to begin
    //Pressing the start button will anchor the worldto its position and start the game
    public void OnStartButton()
    {
        world.gameObject.AddComponent<ARAnchor>();
        gameController.GameResume();
    }
}
