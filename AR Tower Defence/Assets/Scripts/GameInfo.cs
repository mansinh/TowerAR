using UnityEngine;
using TMPro;

/**
 * Display helpful game info at the top of the screen such as ability descriptions
 *@ author Manny Kwong 
 */

public class GameInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _cardText;
    [SerializeField] TMP_Text _gameText;
    public static GameInfo Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetCardText(string info) {
        _cardText.text = ""+info;
    }
}
