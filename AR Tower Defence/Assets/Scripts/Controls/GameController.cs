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
    [SerializeField] UseButton useCardButton;

    public bool IsAR;
    public static GameController Instance;
    private Card _selectedCard;
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
        }
    }

    public void SetSelectedCard(Card selectedCard)
    {
        if (_selectedCard != null)
        {
            _selectedCard.Deselect();
        }
        _selectedCard = selectedCard;
        useCardButton.IsUsingCard = true;
        selectedCard.SetGameInfo();
    }

    public void UseSelectedCard()
    {
        if (_selectedCard != null)
        {
            //Allow card action if cursor is over the player territory
            if (MyCursor.Instance.GetIsActionable())
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
        }
        _selectedCard = null;
        useCardButton.IsUsingCard = false;
        GameInfo.Instance.SetText("");
    }

    //Discard card from hand in return for some points back
    public void Discard()
    {
        if (_selectedCard != null)
        {
            _selectedCard.Discard();
        }
        _selectedCard = null;
        useCardButton.IsUsingCard = false;
        Points.Instance.Discarded();
    }
 
    void FixedUpdate()
    {
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
                
                if (Input.GetMouseButton(0) && MyCursor.Instance.GetIsActionable())
                {
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
        gameOverMenu.alpha = 0;
        gameOverMenu.gameObject.SetActive(true);
        StartCoroutine(UITransitions.AlphaTo(gameOverMenu, 1, 0.3f));
    }
}
