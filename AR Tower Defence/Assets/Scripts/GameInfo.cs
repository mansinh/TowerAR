using UnityEngine;
using TMPro;

/**
 * Display helpful game info at the top of the screen such as ability descriptions
 *@ author Manny Kwong 
 */

[RequireComponent(typeof(TMP_Text))]
public class GameInfo : MonoBehaviour
{
    TMP_Text _text;
    public static GameInfo Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _text = GetComponent<TMP_Text>();
        }
    }

    public void SetText(string info) {
        _text.text = ""+info;
    }
}
