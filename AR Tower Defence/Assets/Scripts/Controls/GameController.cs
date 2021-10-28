using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    [SerializeField] ARSession arSession;
    [SerializeField] ARController arController;
    [SerializeField] Camera testCamera;
    [SerializeField] float zoomSpeed = 1, cameraSpeed = 100;

    [SerializeField] CanvasGroup ARMenu;
    [SerializeField] CanvasGroup HUD;
    [SerializeField] CanvasGroup gameOverMenu;
    [SerializeField] UseButton useCardButton;

    public bool IsAR;
    public static GameController Instance;

    private Card _selectedCard;

    void Awake()
    {
        Instance = this;
        ARMenu.gameObject.SetActive(true);
        HUD.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);

        GamePause();
    }

    void Start()
    {
        if (IsAR)
        {
            arSession.gameObject.SetActive(true);
            arController.gameObject.SetActive(true);
            testCamera.gameObject.SetActive(false);
            ARMenu.gameObject.SetActive(true);
            MyCursor.Instance.SetScreenPosition(screenCenter);
        }
        else
        {
            
            arSession.gameObject.SetActive(false);
            arController.gameObject.SetActive(false);
            ARMenu.gameObject.SetActive(false);

            testCamera.gameObject.SetActive(true);
            HUD.gameObject.SetActive(true);
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
    }

    public void UseSelectedCard()
    {
        if (_selectedCard != null)
        {
            if (MyCursor.Instance.GetIsActionable())
            {
                bool cardUsedUp = _selectedCard.ActivateCard();
                if (cardUsedUp)
                {
                    _selectedCard = null;
                    useCardButton.IsUsingCard = false;
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
    }

    public void RemoveCard()
    {
        if (_selectedCard != null)
        {
            _selectedCard.Remove();
        }
        _selectedCard = null;
        useCardButton.IsUsingCard = false;
        Points.Instance.RemovedCard();
    }

    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
    // Update is called once per frame

    void FixedUpdate()
    {
        if (IsAR)
        {
            MyCursor.Instance.Cast(screenCenter);
        }
        else
        {
            MyCursor.Instance.SetScreenPosition(Input.mousePosition);
            MyCursor.Instance.Cast(Input.mousePosition);
        }
    }


    private void Update()
    {


        if (!IsAR)
        {
            KeyboardControls();
        }
    }

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
        Time.timeScale = 0;
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    public void GameResume()
    {
        BakeNavMesh();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<NavMeshAgent>().enabled = true;
        }
        Time.timeScale = 1;
  
        HUD.gameObject.SetActive(true);
        //StartCoroutine(UITransitions.AlphaTo(HUD, 1, 0.3f));
        StartCoroutine(UITransitions.AlphaTo(ARMenu, 0, 0.3f));
    }


  

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
    public void GameReset()
    {

    }

    public void GameOver()
    {
        gameOverMenu.alpha = 0;
        gameOverMenu.gameObject.SetActive(true);
        StartCoroutine(UITransitions.AlphaTo(gameOverMenu, 1, 0.3f));
    }
}
