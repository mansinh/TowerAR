using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using World;
[RequireComponent(typeof(ARRaycastHit))]
public class ARPlaceWorld : MonoBehaviour
{

    [SerializeField] Transform _ARCursor;
    [SerializeField] Text _trackingButtonText;
    [SerializeField] World.World _worldRoot;
    [SerializeField] ARPlane _planrPrefab;
    ARRaycastManager _arRayCastManager;
    ARPlaneManager _planeManager;
    ARSession _arSession;

    static Vector2 screenCenter = new Vector3(Screen.width, Screen.height) / 2;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool _isPlacing = true;
    Pose targetPose;

    private void Awake()
    {
        _arRayCastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        _arSession = FindObjectOfType<ARSession>();
        _worldRoot.transform.position = new Vector3(0,100,0);
        _isPlacing = true;
        _trackingButtonText.transform.parent.gameObject.SetActive(true);
        Time.timeScale = 0;
        _worldRoot.Pause();
        _trackingButtonText.text = "Anchor World";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlacing)
        {
            if (_arRayCastManager.Raycast(screenCenter, hits, TrackableType.Planes))
            {
                targetPose = hits[0].pose;
                //ARCursor.transform.position = targetPose.position;
                //ARCursor.transform.rotation = targetPose.rotation;

                _worldRoot.transform.position = targetPose.position;
                //_worldRoot.transform.rotation = targetPose.rotation;
            }
            DragToRotate();
        }

    }
    public void ToggleTracking() {      
        if (!_isPlacing)
        {
            Time.timeScale = 0;
            _trackingButtonText.text = "Anchor World";        
            Destroy(_worldRoot.GetComponent<ARAnchor>());
            _worldRoot.Pause();
           
        }
        else {
            _trackingButtonText.text = "Reposition World";
                  
            _worldRoot.gameObject.AddComponent<ARAnchor>();           
            _worldRoot.Refresh();
            Time.timeScale = 1;
        }
        SetPlanesActive(!_isPlacing);
        _isPlacing = !_isPlacing;
    }

    void SetPlanesActive(bool isActive) {
        _planeManager.enabled = isActive;
        _planrPrefab.gameObject.SetActive(isActive);
        _planeManager.SetTrackablesActive(isActive);
        
    }

    bool _isDragging = false;
    Vector3 _dragFrom = Vector3.zero;
    float _rotateFrom = 0;
    float _zoomFrom = 1;
    void DragToRotate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rotateFrom = _worldRoot.transform.eulerAngles.y;
            _zoomFrom = _worldRoot.transform.localScale.x;
            _dragFrom = Input.mousePosition;
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
        if (_isDragging)
        {
            float deltaRotate = Mathf.Sign(Input.mousePosition.x-Screen.width/2) *(_dragFrom.y-Input.mousePosition.y)/ Screen.height*360;
            _worldRoot.transform.eulerAngles = new Vector3(0, _rotateFrom + deltaRotate, 0);

            //float deltaZoom = (_dragFrom.x - Input.mousePosition.x) / Screen.width;
           //worldRoot.transform.localScale = Vector3.one * Mathf.Max(_zoomFrom+deltaZoom,0.1f);
           
        }
    }
}
