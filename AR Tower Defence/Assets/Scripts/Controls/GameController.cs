using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] ARSession arSession;
    [SerializeField] ARPlaceWorld arPlaceWorld;
    [SerializeField] Camera testCamera;
    [SerializeField] float zoomSpeed = 1, cameraSpeed = 100;
    [SerializeField] World world;

    [SerializeField] CanvasGroup ARMenu;
    [SerializeField] CanvasGroup HUD;
    [SerializeField] CanvasGroup gameOverMenu;
    [SerializeField] UseButton useCardButton;

    public bool IsAR;
    public static GameController Instance;

    private Card _selectedCard;

    void Awake()
    {
        Time.timeScale = 0;
        ARMenu.gameObject.SetActive(true);
        HUD.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        Instance = this;
    }

    void Start()
    {
        if (IsAR)
        {
            arSession.gameObject.SetActive(true);
            arPlaceWorld.gameObject.SetActive(true);
            testCamera.gameObject.SetActive(false);
            ARMenu.gameObject.SetActive(true);
            MyCursor.Instance.SetScreenPosition(screenCenter);
        }
        else
        {
            Time.timeScale = 1;
            arSession.gameObject.SetActive(false);
            arPlaceWorld.gameObject.SetActive(false);
            testCamera.gameObject.SetActive(true);
            HUD.gameObject.SetActive(true);
            ARMenu.gameObject.SetActive(false);
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
        world.Pause();
    }

    public void GameResume()
    {
        Time.timeScale = 1;
        HUD.alpha = 0;
        HUD.gameObject.SetActive(true);
        StartCoroutine(UITransitions.AlphaTo(HUD, 1, 0.3f));
        StartCoroutine(UITransitions.AlphaTo(ARMenu, 0, 0.3f));
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
