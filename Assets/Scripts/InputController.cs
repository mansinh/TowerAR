using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    [SerializeField] GameObject ARCamera, PCCamera;
    [SerializeField] Texture2D cursorRed, cursorWhite, cursorYellow;
    [SerializeField] Image cursorAR;
    [SerializeField] GameObject squareSelect;

    Vector2 cursorHotspot;
    public GameObject cursor;
    World world;
    SpellController spells;
    UnitController units;

    public bool isAR = true;
    bool placeable = false;
    bool worldPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
        spells = FindObjectOfType<SpellController>();
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
        cursorHotspot = new Vector2(cursorWhite.width, cursorWhite.height) / 2;

    }

    // Update is called once per frame
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
            transform.eulerAngles += new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * 100, 0);
            PCCamera.transform.eulerAngles -= new Vector3(Input.GetAxis("Vertical") * Time.deltaTime * 10,0, 0);
        }
        CastCursor();
    }

    RaycastHit hit;
    bool isHitting;
    void CastCursor()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        isHitting = Physics.Raycast(ray, out hit);
        if (isHitting)
        {
            string hitTag = hit.collider.tag;
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
                        ShowSelectedSquare(hit);
                        if (Input.GetMouseButtonUp(1) && squareSelect.active)
                        {
                            PlaceUnit();
                        }
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
        Block block = hit.collider.GetComponent<Block>();
        print(block);
        if (block) {
            squareSelect.SetActive(true);
            squareSelect.transform.parent = block.transform;          
            squareSelect.transform.localPosition = hit.normal*0.55f;
            squareSelect.transform.forward = -hit.normal;
        }
    }

    public void CastLightning()
    {
        if(isHitting)
        spells.CastLightning(hit);
    }

    void PlaceUnit()
    {
        units.PlaceUnit(squareSelect.transform);
    }

    void UpdateCursor()
    {
        Vector3 position = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            placeable = true;
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
        else
        {
            placeable = false;
        }

    }
}


/*
   void CastCursor()
   {

       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       bool isHitting = Physics.Raycast(ray, out hit);
       if (isHitting)
       {
           string hitTag = hit.collider.tag;
           switch (hitTag)
           {
               case "Enemy":
                   {
                       Cursor.SetCursor(cursorRed, cursorHotspot, CursorMode.Auto);
                       HideSelectedSquare();
                       break;
                   }
               case "Placeable":
                   {
                       Cursor.SetCursor(cursorYellow, cursorHotspot, CursorMode.Auto);
                       ShowSelectedSquare(hit);
                       if (Input.GetMouseButtonUp(1) && squareSelect.active)
                       {
                           PlaceUnit();
                       }
                       break;
                   }
               default:
                   {
                       Cursor.SetCursor(cursorWhite, cursorHotspot, CursorMode.Auto);
                       HideSelectedSquare();
                       break;
                   }
           }

           if (Input.GetMouseButtonUp(0))
           {
               CastSpell(hit);
           }
       }
       else
       {
           HideSelectedSquare();
           Cursor.SetCursor(cursorWhite, cursorHotspot, CursorMode.Auto);
       } 
   }
   */
