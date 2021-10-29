using UnityEngine;
using UnityEngine.UI;

/**
 * Slider that sets the speed of time the game runs at
 *@ author Manny Kwong 
 */

public class TimeController : MonoBehaviour
{
    Slider _slider;
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetTimeScale() {
        Time.timeScale = _slider.value;
    } 
}
