using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameController : MonoBehaviour
{
    [SerializeField] ARSession _arSession;
    [SerializeField] ARPlaceWorld _arPlaceWorld;
    [SerializeField] Camera _testCamera;
    [SerializeField] float _zoomSpeed = 1, _cameraSpeed = 100;
    [SerializeField] WorldRoot _worldRoot;
    [SerializeField] GameObject _gameOverView;

    public bool IsAR;
    LightningController _lightningController;
    [SerializeField] Player _player;


    public static GameController Instance;

    void Awake()
    {
        Time.timeScale = 0;

        Instance = this;
        _lightningController = FindObjectOfType<LightningController>();

    }

    IEnumerator Start()
    {
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        IsAR = ARSession.state != ARSessionState.Unsupported;
        if (IsAR)
        {
            _arSession.gameObject.SetActive(true);
            _arPlaceWorld.gameObject.SetActive(true);
            _testCamera.gameObject.SetActive(false);
            MyCursor.instance.SetScreenPosition(screenCenter);
        }
        else
        {
            Time.timeScale = 1;
            _arSession.gameObject.SetActive(false);
            _arPlaceWorld.gameObject.SetActive(false);
            _testCamera.gameObject.SetActive(true);
        }

    }



    public void GameStart()
    {

    }

    public void GamePause()
    {

    }

    public void GameResume()
    {

    }

    public void GameReset()
    {

    }

    public void GameOver()
    {

    }

    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsAR)
        {

            MyCursor.instance.Cast(screenCenter);
        }
        else
        {
            MyCursor.instance.SetScreenPosition(Input.mousePosition);
            MyCursor.instance.Cast(Input.mousePosition);

        }
    }


    private void Update()
    {
        if (IsAR)
        {
            

        }
        else
        {
            //KeyboardControls()
            
        }
       
        if (_player.IsDestroyed)
        {
            GameOver();
        }

    }

   


    void KeyboardControls() {
        transform.eulerAngles += new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0) * _cameraSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.position += Camera.main.transform.position * _zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.position -= Camera.main.transform.position * _zoomSpeed * Time.deltaTime;
        }
    }
}
