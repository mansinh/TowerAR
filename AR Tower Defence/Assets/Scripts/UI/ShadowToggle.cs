using UnityEngine;
using UnityEngine.UI;
public class ShadowToggle : MonoBehaviour
{
    Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    private void OnEnable()
    {
        if (Options.Instance)
            toggle.isOn = Options.Instance.shadows;
    }

    public void SetShadows(bool shadows)
    {
        if (Options.Instance)
            Options.Instance.shadows = shadows;
    }
}
