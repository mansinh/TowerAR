using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DayNightLighting", menuName = "Scriptables/DayNightLighting", order =1)]
public class DayNightLighting : ScriptableObject
{
    public AnimationCurve LightIntensity;
    public AnimationCurve LightDirection;
    public Gradient AmbientColour;
    
}
