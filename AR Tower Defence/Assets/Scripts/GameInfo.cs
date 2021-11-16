using UnityEngine;
using TMPro;

/**
 * Display helpful game info at the top of the screen such as ability descriptions
 *@ author Manny Kwong 
 */

public class GameInfo : MonoBehaviour
{
    [SerializeField] TMP_Text gameText;

    public static GameInfo Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetSelectedText(string info) {
        gameText.text = ""+info;
    }
    public void SetHoverText(string info)
    {
        if (!GameController.Instance.IsSomethingSelected())
        {
            gameText.text = "" + info;
        }
    }
}
