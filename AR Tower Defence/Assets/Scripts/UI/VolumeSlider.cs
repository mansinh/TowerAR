using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        if(Options.Instance)
        slider.value = Options.Instance.volume;
    }

    public void SetVolume(float volume)
    {
        if (Options.Instance)
            Options.Instance.volume = volume;
    }
}
