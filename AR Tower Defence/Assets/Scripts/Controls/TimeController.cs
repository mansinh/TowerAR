using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
