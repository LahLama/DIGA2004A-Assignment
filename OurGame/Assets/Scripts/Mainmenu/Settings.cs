using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public GameObject settingsPanel;
    public Slider lookSensitivitySlider;
    public static float LookSensitivity = 1.0f;


    void Start()
    {
        // Load saved values
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        masterVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);


        // Apply volumes
        AudioListener.volume = masterVolumeSlider.value;

         // Load saved sensitivity
        LookSensitivity = PlayerPrefs.GetFloat("LookSensitivity", 1f);
        lookSensitivitySlider.value = LookSensitivity;
    }

    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        //  control a music AudioSource 
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        //  control a SFX AudioSource here
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

     public void OnSensitivityChanged(float value)
    {
        LookSensitivity = value;
        PlayerPrefs.SetFloat("LookSensitivity", value);
    }
 
}