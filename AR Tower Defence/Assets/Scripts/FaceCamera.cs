using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FaceCamera : MonoBehaviour
{
    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.IsAR)
        {
            cameraTransform = FindObjectOfType<ARCameraManager>().transform;
        }
        else
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toCamera = Quaternion.LookRotation(cameraTransform.position - transform.position, Vector3.up).eulerAngles;
        toCamera.x = 0;
        transform.eulerAngles = toCamera;
    }
}
