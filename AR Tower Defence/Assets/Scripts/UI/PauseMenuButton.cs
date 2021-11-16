using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public void OnPause()
    {

        if (!pauseMenu.active) { Time.timeScale = 0; pauseMenu.gameObject.SetActive(true); } 
        else if (pauseMenu.active) { Time.timeScale = 1;pauseMenu.gameObject.SetActive(false); }
    }
}
