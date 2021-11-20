using UnityEngine;

/**
 * Store the options settings across scenes. Adjust volume with a slider and toggle shadows on and off.
 *@ author Manny Kwong 
 */

public class Options : MonoBehaviour
{
    public static Options Instance { get; private set; }

    public float volume = 1;
    public bool shadows = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
