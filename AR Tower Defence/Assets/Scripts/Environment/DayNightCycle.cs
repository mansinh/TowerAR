using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Light))]
public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light _sun;
    
    [SerializeField] private float _dayLength = 10;
    [SerializeField, Range(0, 1)] private float _dayTime;
    [SerializeField] private float _sunIntensity;
    [SerializeField] private float _ambientIntensity;
    public AnimationCurve Intensity;
    public AnimationCurve Direction;
    public Gradient SunColour;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate()
    {
      
        UpdateLighting();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) {
            _dayTime += Time.deltaTime/_dayLength;
            _dayTime %= 1;
            UpdateLighting();
        }
       
    }

    void UpdateLighting() {
        
        RenderSettings.ambientLight = SunColour.Evaluate(_dayTime)* _ambientIntensity;     
        _sun.intensity = Intensity.Evaluate(_dayTime) * _sunIntensity;
        _sun.transform.localEulerAngles = new Vector3(Direction.Evaluate(_dayTime)*90, 90, 0);
    }
}
