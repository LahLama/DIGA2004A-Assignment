using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    public Slider lookSensitivitySlider;
    public static float LookSensitivity = 1.0f;

    void Start()
    {
        // Load saved sensitivity
        LookSensitivity = PlayerPrefs.GetFloat("LookSensitivity", 1.0f);
        lookSensitivitySlider.value = LookSensitivity;
    }

    public void OnSensitivityChanged(float value)
    {
        LookSensitivity = value;
        PlayerPrefs.SetFloat("LookSensitivity", value);
    }
}