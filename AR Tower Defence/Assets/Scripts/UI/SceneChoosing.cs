using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChoosing : MonoBehaviour
{
    [SerializeField] string levelName = "";
    public void SceneLoading()
    {
        SceneManager.LoadScene(levelName);
    }
}
