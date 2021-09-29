using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    [SerializeField] GameObject ARCamera, PCCamera;
    [SerializeField] Image cursorAR;
    [SerializeField] GameObject squareSelect;

    Vector2 cursorHotspot;
    public GameObject cursor;
    World world;
    UnitController units;

    public bool isAR = true;
    bool placeable = false;
    bool worldPlaced = false;




    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();

        units = FindObjectOfType<UnitController>();

        if (isAR)
        {

            ARCamera.SetActive(true);
            PCCamera.SetActive(false);
            world.gameObject.SetActive(false);
        }
        else
        {

            ARCamera.SetActive(false);
            PCCamera.SetActive(true);
            world.gameObject.SetActive(true);
            world.Init();
        }

    }


    void Update()
    {

        if (isAR)
        {
            if (!worldPlaced)
            {
                if (!planeManager.enabled)
                {
                    planeManager.enabled = true;
                }
                UpdateCursor();
                if (Input.touchCount > 0 && placeable)
                {
                    world.transform.position = cursor.transform.position;

                    world.transform.up = -cursor.transform.forward;

                    world.gameObject.SetActive(true);
                    world.Init();

                    //planeManager.enabled = false;
                    worldPlaced = true;
                }
            }
        }
        else
        {
            transform.eulerAngles -= new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * 200, 0);
            PCCamera.transform.eulerAngles -= new Vector3(Input.GetAxis("Vertical") * Time.deltaTime * 15, 0, 0);
        }
        CastCursor();
    }

    RaycastHit cursorHit;
    bool isCursorHitting;

    public RaycastHit GetCursorHit()
    {

        return cursorHit;
    }
    public bool GetCursorHitting()
    {
        return isCursorHitting;
    }

    void CastCursor()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        isCursorHitting = Physics.Raycast(ray, out cursorHit);
        if (isCursorHitting)
        {
            string hitTag = cursorHit.collider.tag;
            switch (hitTag)
            {
                case "Enemy":
                    {
                        cursorAR.color = Color.magenta;
                        HideSelectedSquare();
                        break;
                    }
                case "Placeable":
                    {
                        cursorAR.color = Color.yellow;
                        ShowSelectedSquare(cursorHit);
                      
                        break;
                    }
                default:
                    {
                        cursorAR.color = Color.white;
                        HideSelectedSquare();
                        break;
                    }
            }
        }
        else
        {

            cursorAR.color = Color.white;
            HideSelectedSquare();
        }
    }


    void HideSelectedSquare()
    {
        squareSelect.SetActive(false);
    }
    void ShowSelectedSquare(RaycastHit hit)
    {

        squareSelect.SetActive(true);
        Vector3 p = world.transform.InverseTransformPoint(hit.point);
        
        p = new Vector3(Mathf.Round(p.x/2)*2, Mathf.Round(p.y / 2) * 2, Mathf.Round(p.z / 2) * 2);
        squareSelect.transform.localPosition = p + hit.normal / 100;
        squareSelect.transform.forward = -hit.normal;

    }



    ARTrackable trackable;
    void UpdateCursor()
    {
        Vector3 position = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            placeable = true;
            trackable = hits[0].trackable;
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
        else
        {
            placeable = false;
        }

    }
}

