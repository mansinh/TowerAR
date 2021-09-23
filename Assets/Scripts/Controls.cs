using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    World world;

    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlsPC();
    }

    void ControlsPC() {
        
        transform.eulerAngles += new Vector3(0,Input.GetAxis("Horizontal")*Time.deltaTime*100,0);
    }
}
