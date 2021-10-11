using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameController : MonoBehaviour
{
    [SerializeField] ARSession arSession;
    [SerializeField] ARPlaceWorld arPlaceWorld;
    [SerializeField] Camera testCamera;
    public bool IsAR;
    LightningController lightningController;

    [SerializeField] float zoomSpeed = 1, cameraSpeed = 100;

    public static GameController instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            lightningController = FindObjectOfType<LightningController>();
            IsAR = WebCamTexture.devices.Length > 0;
            if (IsAR)
            {
                arSession.gameObject.SetActive(true);
                arPlaceWorld.gameObject.SetActive(true);
                testCamera.gameObject.SetActive(false);
            }
            else
            {

                arSession.gameObject.SetActive(false);
                arPlaceWorld.gameObject.SetActive(false);
                testCamera.gameObject.SetActive(true);
            }
        }        
    }

    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0)/2;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsAR)
        {
            MyCursor.instance.Cast(screenCenter);
        }
        else {
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
    }

}
