using UnityEngine;

/**
 * Controls the timing and lighting of the day-night cycle
 *@ author Manny Kwong 
 */

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
    public bool isDay = false;
    [SerializeField, Range(0, 1)] private float timeOfDay;
    [SerializeField] private int dayCount = 0;
    
    public static DayNightController Instance;

    private void Awake()
    {
        Instance = this;
        if (Options.Instance)
        {
            SetShadows(Options.Instance.shadows);
        }
    }

    private void OnValidate()
    {
        UpdateLighting();
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            timeOfDay = startTime;
            UpdateLighting();
        }
        else
        {
            timeOfDay = 0.4f;
            UpdateLighting();
        }
    }

    public void SetTime(float t)
    {
        timeOfDay = t;
        UpdateLighting();
    }

    void Update()
    {
        if (Application.isPlaying) {
            timeOfDay += Time.deltaTime/dayLength;
           
            UpdateLighting();

            if (timeOfDay > dawnTime && timeOfDay < duskTime && !isDay)
            {
                Dawn();
            }            
            if (timeOfDay > duskTime && isDay)
            {
                Dusk();
            }
            if (timeOfDay > 1)
            {
                Midnight();
            }
        }  
    }

    //Set ambient light colour, sun light intensity and sun light direction over time of day
    void UpdateLighting() {     
        RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay)* ambientIntensity;     
        sun.intensity = intensity.Evaluate(timeOfDay) * sunIntensity;
        sun.transform.localEulerAngles = new Vector3(direction.Evaluate(timeOfDay)*90, 30, 0);
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

    public void SetShadows(bool isOn)
    {
        if (isOn)
        {
            sun.shadows = LightShadows.Soft;
        }
        else
        {
            sun.shadows = LightShadows.None;
        }       
    }
}
