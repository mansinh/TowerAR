using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        StartCoroutine(UITransitions.AlphaTo(GetComponent<CanvasGroup>(), 0, 0.3f));
    }
}