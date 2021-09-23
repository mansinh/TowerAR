using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainTitleUI : MonoBehaviour
{
    [SerializeField] Button startButton, exitButton, themeButton;
    VisualElement root;
    [SerializeField] StyleSheet lightTheme, darkTheme;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Query<Button> ("StartButton");
        exitButton = root.Query<Button>("ExitButton");
        themeButton = root.Query<Button>("ThemeButton");

     
     
        startButton.clicked += OnStart;
        exitButton.clicked += OnExit;
        themeButton.clicked += OnTheme;
    }

    void OnStart() {
        SceneManager.LoadScene("Game");
    }
    void OnExit()
    {
        print("Quit");
        Application.Quit();
    }
    void OnTheme()
    {
        print("change theme to");
        
        VisualElementStyleSheetSet sheet = root.styleSheets;
        if (sheet.Contains(darkTheme))
        {
            sheet.Remove(darkTheme);
            sheet.Add(lightTheme);
            print("light theme");
        }
        else {
            sheet.Remove(lightTheme);
            sheet.Add(darkTheme);
            print("dark theme");
        }
        
        
    }
}
