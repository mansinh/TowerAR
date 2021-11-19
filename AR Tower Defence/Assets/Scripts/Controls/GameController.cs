using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AI;

/**
 * Manages player input and game states
 * Might have to separate this into 2 different classes in the future
 * TODO game pause menu, game restart, game options, return to AR menu, exit game
 *@ author Manny Kwong 
 */

public class GameController : MonoBehaviour
{
    [SerializeField] ARSession arSession;
    [SerializeField] ARController arController;
    [SerializeField] Camera testCamera;
    [SerializeField] float zoomSpeed = 1, cameraSpeed = 100;

    [SerializeField] CanvasGroup arMenu;
    [SerializeField] CanvasGroup hud;
    [SerializeField] CanvasGroup gameOverMenu;
    [SerializeField] CanvasGroup gameWonMenu;
    [SerializeField] UseButton useButton;

    public bool IsAR;
    public static GameController Instance;
    private Card _selectedCard;
    private ISelectable _selectedObject;
    private IHoverable _hoveredObject;
    public bool IsHoldingObject;
    public Transform cameraTransform;
    private Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

    void Awake()
    {
        Instance = this;
        arMenu.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        GamePause();
    }

    void Start()
    {
        IsAR = Application.platform == RuntimePlatform.Android;
        if (IsAR)
        {
            //Turn on AR
            arSession.gameObject.SetActive(true);
            arController.gameObject.SetActive(true);
            arMenu.gameObject.SetActive(true);

            //Turn off test camera
            testCamera.gameObject.SetActive(false);

            //Set cursor to center of the screen
            MyCursor.Instance.SetScreenPosition(screenCenter);

            //Set camera transform to the AR session origin
            cameraTransform = arController.transform;
        }
        else
        {
            //Turn off AR for PC testing
            arSession.gameObject.SetActive(false);
            arController.gameObject.SetActive(false);
            arMenu.gameObject.SetActive(false);

            //Turn on test camera
            testCamera.gameObject.SetActive(true);

            //Start game
            hud.gameObject.SetActive(true);
            GameResume();

            //Set camera transform to the pc test camera
            cameraTransform = testCamera.transform;
        }
    }

    public void SetSelectedCard(Card selectedCard)
    {
        if (_selectedCard != null)
        {
            _selectedCard.Deselect();
        }
        _selectedCard = selectedCard;
        useButton.SetIsUsingCard(selectedCard);
        selectedCard.SetGameInfo();
    }

    public bool GetIsCardUsable()
    {
        if (_selectedCard == null)
        {
            return false;
        }
        return _selectedCard.GetIsUsable();
    }

    public Card GetSelectedCard()
    {
        return _selectedCard;
    }

    public void UseSelectedCard()
    {
        if (_selectedCard != null)
        {
            //Allow card action if cursor is over the player territory
            if (GetIsCardUsable())
            {
                //Deselect card when it is used up
                bool cardUsedUp = _selectedCard.ActivateCard();
                if (cardUsedUp)
                {
                    DeselectCard();
                }
            }
        }
    }

    public void DeselectCard()
    {
        if (_selectedCard != null)
        {
            _selectedCard.Deselect();
            _selectedCard = null;
        }    
        useButton.SetIsUsingCard(null);
        GameInfo.Instance.SetSelectedText("");
    }


    public void DeselectCard(Card selected)
    {
        if (_selectedCard == selected)
        {
            _selectedCard = null;
            DeselectCard();
        }
    }


    //Discard card from hand in return for some points back
    public void Discard()
    {
        if (_selectedCard != null)
        {
            _selectedCard.Discard();
            Points.Instance.Discarded(_selectedCard);
        }
        _selectedCard = null;
        useButton.SetIsUsingCard(null);
        
    }

   

    //Rotate ghost of building card
    public void Rotate(float value)
    {
        if (_selectedCard != null)
        {
            if (_selectedCard.GetComponent<BuildingCard>())
            {
                _selectedCard.GetComponent<BuildingCard>().SetRotation(value * 360 / 16);
            }
        }
    }

    private void Update()
    {
        //Controls for testing on PC
        if (!IsAR)
        {
            KeyboardControls();
            //Activate selected card if left mouse button is held
            if (_selectedCard)
            {
                if (Input.GetMouseButton(0) && GetIsCardUsable())
                {
                    print("activation" + _selectedCard);
                    _selectedCard.ActivateCard();
                }
            }
            if (_selectedCard)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    _selectedCard.DeactivateCard();
                }
            }
        }

        //Find out what is in front of the cursor
        if (IsAR)
        {
            //Cursor fixed at screen center if AR
            MyCursor.Instance.Cast(screenCenter);
        }
        else
        {
            //Cursor follows mouse if testing on PC
            MyCursor.Instance.SetScreenPosition(Input.mousePosition);
            MyCursor.Instance.Cast(Input.mousePosition);
        }


        if (MyCursor.Instance.GetCursorHitting())
        {
            IHoverable hoverHit = MyCursor.Instance.GetCursorHit().collider.GetComponent<IHoverable>();
            if (hoverHit != null)
            {
                if (hoverHit != _hoveredObject)
                {
                    EnterHover(hoverHit);
                }
                else if (_hoveredObject != null)
                {
                    _hoveredObject.OnHoverStay();
                }
            }
            else
            {
                LeaveHover();
            }

            if (_selectedObject == null)
            {
                if (_selectedCard == null)
                {
                    if (Input.GetMouseButtonDown(0) && !IsAR)
                    {
                        useButton.OnClick();
                    }
                }
            }
            else
            {
                _selectedObject.UpdateSelected();
                if (Input.GetMouseButtonDown(0) && !IsAR)
                {
                    useButton.OnClick();
                }
            }
        }
        else
        {
            LeaveHover();
        }
    }


    public void SelectObject()
    {
        if (_selectedCard != null)
        {
            return;
        }
        if (_hoveredObject == null)
        {
            return;
        }
        DeselectObject();
        if (_hoveredObject.GetSelectable() != null)
        {
            _hoveredObject.GetSelectable().Select();
            
            _selectedObject = _hoveredObject.GetSelectable();
            IsHoldingObject = true;
            if (_selectedObject.UseImmediately())
            {
                useButton.Off();
                _selectedObject.Use();
                DeselectObject();
                
            }
        }
    }
    public void SelectObject(ISelectable selectable)
    {
        DeselectObject();
        selectable.Select();
        _selectedObject = selectable;
        IsHoldingObject = true;
        useButton.On();
    }
    
    public void DeselectObject()
    {
        if (_selectedObject != null)
        {
            _selectedObject.Deselect();
            _selectedObject = null;
            IsHoldingObject = false;
        }  
    }

    //Destroy selected object
    public void DestroySelectedObject()
    {
        if (_selectedObject != null)
        {
            _selectedObject.Destroy();
            Points.Instance.Sacrifice(_selectedObject);
            _selectedObject = null;
            IsHoldingObject = false;
            useButton.Off();
        }
    }
    //Use object and return whether the object should be deselected
    public bool UseObject()
    {
        if (_selectedObject != null)
        {
            return _selectedObject.Use();
        }
        return true;
    }


    void EnterHover(IHoverable hoverHit)
    {
        if (_hoveredObject != null)
        {
            _hoveredObject.OnHoverLeave();
        }
        hoverHit.OnHoverEnter();
        _hoveredObject = hoverHit;
        if (!IsHoldingObject)
        {
            useButton.SetHoveringSelectable(_hoveredObject.GetSelectable());
        }
    }
    void LeaveHover()
    {
        if (_hoveredObject != null)
        {
            _hoveredObject.OnHoverLeave();
            _hoveredObject = null;
        }
        if (!IsHoldingObject)
        {
            useButton.SetHoveringSelectable(null);
        }
    }

    //Rotate world about its center vertically and horizontally with keyboard controls
    void KeyboardControls()
    {
        transform.eulerAngles += new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0) * cameraSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.position += Camera.main.transform.position * zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.position -= Camera.main.transform.position * zoomSpeed * Time.deltaTime;
        }
    }

    public void GamePause()
    {
        //Pause time
        Time.timeScale = 0;

        //Turn off NavMeshAgents
        NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i].enabled = false;
        }

    }

    public void GameResume()
    {
        //Rebake nav meshes in case game world was repositioned and turn on NavMeshAgents 
        BakeNavMesh();
        NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i].enabled = true;
        }

        //Set time to normal
        Time.timeScale = 1;

        //Turn on the head up display if not already on
        hud.gameObject.SetActive(true);

        //Fade out the AR menu 
        StartCoroutine(UITransitions.AlphaTo(arMenu, 0, 0.3f));
    }

    //Bakes NavMesh needed for pathfinding
    public void BakeNavMesh()
    {
        NavMeshSurface[] navSurfaces = FindObjectsOfType<NavMeshSurface>();
        foreach (NavMeshSurface navSurface in navSurfaces)
        {
            if (navSurface.isActiveAndEnabled)
            {
                print("Bake Mesh");
                navSurface.RemoveData();
                navSurface.BuildNavMesh();
            }
        }
    }

    //Fades in the game over overlay when player (shrine) is destroyed
    public void GameOver()
    {

        gameOverMenu.gameObject.SetActive(true);
       
    }

    //Fades in the game won overlay when all portals are destroyed
    public void GameWon()
    {    
        gameWonMenu.gameObject.SetActive(true);  
    }

    public bool IsSomethingSelected()
    {
        return _selectedObject != null || _selectedCard != null;
    }
}
