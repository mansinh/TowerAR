using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    Button startButton, exitButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("StartButton");
        exitButton = root.Q<Button>("ExitButton");

        startButton.clicked += OnStartButton;
        exitButton.clicked += OnExitButton;
    }

    void OnStartButton()
    {
        SceneManager.LoadScene("Game");
    }
    void OnExitButton()
    {
        print("Quit");
        Application.Quit();
    }
}
