using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Light))]
public class DayNightController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private float dayLength = 10;
   
    [SerializeField] private float sunIntensity;
    [SerializeField] private float ambientIntensity;

    [SerializeField] private AnimationCurve intensity;
    [SerializeField] private AnimationCurve direction;
    [SerializeField] private Gradient ambientColor;

   
    [SerializeField] private float startTime = 0.25f;
    [SerializeField] private float dawnTime = 0.3f;
    [SerializeField] private float duskTime = 0.75f;

    [Space(10)]
    [Header("Day Status")]
    [Header("______________________________________________________________________")]
    [SerializeField] private bool isDay = false;
    [SerializeField, Range(0, 1)] private float timeOfDay;
    [SerializeField] private int dayCount = 0;
    

    private void OnValidate()
    {
        UpdateLighting();
    }

    private void Start()
    {
        timeOfDay = startTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) {
            timeOfDay += Time.deltaTime/dayLength;
           
            UpdateLighting();

            if (timeOfDay > dawnTime && timeOfDay < 0.5 && !isDay)
            {
                Dawn();
            }            
            if (timeOfDay > duskTime && timeOfDay> 0.5 && isDay)
            {
                Dusk();
            }
            if (timeOfDay > 1)
            {
                Midnight();
            }
        }  
    }

    void UpdateLighting() {     
        RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay)* ambientIntensity;     
        sun.intensity = intensity.Evaluate(timeOfDay) * sunIntensity;
        sun.transform.localEulerAngles = new Vector3(direction.Evaluate(timeOfDay)*90, 90, 0);
    }

    void Dawn()
    {
        isDay = true;
        ResetWave();
    }

    void Midnight()
    {
        timeOfDay = 0;
        dayCount++;
    }

    void Dusk()
    {
        isDay = false;
        StartWave();
    }

    void StartWave()
    {
        EnemySource[] _enemySources = FindObjectsOfType<EnemySource>(); 
        foreach (EnemySource source in _enemySources)
        {
            source.StartWave(dayCount);
        }   
    }

    void ResetWave()
    {
        EnemySource[] _enemySources = FindObjectsOfType<EnemySource>();
        foreach (EnemySource source in _enemySources)
        {
            source.ResetWave();
        }
    }
}
