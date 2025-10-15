using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Sensitivity Sliders")]
    public Slider keyboardSensitivitySlider;
    public Slider controllerSensitivitySlider;
    public Slider mouseSensitivitySlider;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Settings Panel")]
    public GameObject settingsPanel;

    public enum InputMethod { Keyboard, Controller, Mouse }
    public InputMethod currentInputMethod = InputMethod.Keyboard;

    public static float LookSensitivity = 1f;
    public static float KeyboardSensitivity = 1f;
    public static float ControllerSensitivity = 1f;
    public static float MouseSensitivity = 1f;

    void Start()
    {
        // Load volume settings
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterVolumeSlider.value = masterVol;
        musicVolumeSlider.value = musicVol;
        sfxVolumeSlider.value = sfxVol;

        AudioListener.volume = masterVol;
        if (musicSource != null) musicSource.volume = musicVol;
        if (sfxSource != null) sfxSource.volume = sfxVol;

        // Load sensitivity settings
        KeyboardSensitivity = PlayerPrefs.GetFloat("KeyboardSensitivity", 1f);
        ControllerSensitivity = PlayerPrefs.GetFloat("ControllerSensitivity", 1f);
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        keyboardSensitivitySlider.value = KeyboardSensitivity;
        controllerSensitivitySlider.value = ControllerSensitivity;
        mouseSensitivitySlider.value = MouseSensitivity;

       // ApplyCurrentSensitivity();
        UpdateSensitivityUI();
    }

    // Volume Handlers
    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;

        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // Sensitivity Handlers
    public void OnKeyboardSensitivityChanged(float value)
    {
        KeyboardSensitivity = value;
        PlayerPrefs.SetFloat("KeyboardSensitivity", value);
        if (currentInputMethod == InputMethod.Keyboard)
            ApplyCurrentSensitivity();
    }

    public void OnControllerSensitivityChanged(float value)
    {
        ControllerSensitivity = value;
        PlayerPrefs.SetFloat("ControllerSensitivity", value);
       if (currentInputMethod == InputMethod.Controller)
            ApplyCurrentSensitivity();
    }

    public void OnMouseSensitivityChanged(float value)
    {
        MouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        if (currentInputMethod == InputMethod.Mouse)
            ApplyCurrentSensitivity();
    }

    public void SetInputMethod(int methodIndex)
    {
        currentInputMethod = (InputMethod)methodIndex;
        ApplyCurrentSensitivity();
        UpdateSensitivityUI();
    }

    void ApplyCurrentSensitivity()
    {
        switch (currentInputMethod)
        {
            case InputMethod.Keyboard:
                LookSensitivity = KeyboardSensitivity;
                break;
            case InputMethod.Controller:
                LookSensitivity = ControllerSensitivity;
                break;
            case InputMethod.Mouse:
                LookSensitivity = MouseSensitivity;
                break;
        }
    }

    void UpdateSensitivityUI()
    {
        keyboardSensitivitySlider.gameObject.SetActive(currentInputMethod == InputMethod.Keyboard);
        controllerSensitivitySlider.gameObject.SetActive(currentInputMethod == InputMethod.Controller);
        mouseSensitivitySlider.gameObject.SetActive(currentInputMethod == InputMethod.Mouse);
    }

    // Panel Controls
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}