using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    NavManager navManager;
   
    public void Init()
    {
        navManager = FindObjectOfType<NavManager>();
        navManager.Bake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
