using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;       // Slider for controlling master volume
    public Slider musicVolumeSlider;        // Slider for controlling music volume
    public Slider sfxVolumeSlider;          // Slider for controlling sound effects volume

    [Header("Sensitivity Sliders")]
    public Slider keyboardSensitivitySlider;    // Slider for keyboard look sensitivity
    public Slider controllerSensitivitySlider;  // Slider for controller look sensitivity
    public Slider mouseSensitivitySlider;       // Slider for mouse look sensitivity

    [Header("Audio Sources")]
    public AudioSource musicSource;         // AudioSource for music playback
    public AudioSource sfxSource;           // AudioSource for sound effects playback

    [Header("Settings Panel")]
    public GameObject settingsPanel;        // Reference to the settings UI panel

    // Enum to represent different input methods
    public enum InputMethod { Keyboard, Controller, Mouse }
    // public InputMethod currentInputMethod = InputMethod.Keyboard; // Currently selected input method (commented out)

    // Static sensitivity values accessible globally
    public static float LookSensitivity = 1f;
    public static float KeyboardSensitivity = 1f;
    public static float ControllerSensitivity = 1f;
    public static float MouseSensitivity = 1f;

    void Start()
    {
        // Load saved volume settings from PlayerPrefs
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Apply loaded values to sliders
        masterVolumeSlider.value = masterVol;
        musicVolumeSlider.value = musicVol;
        sfxVolumeSlider.value = sfxVol;

        // Apply volume settings to audio sources
        ApplyVolumeSettings();

        AudioListener.volume = masterVol;
        if (musicSource != null) musicSource.volume = musicVol;
        if (sfxSource != null) sfxSource.volume = sfxVol;

        // Load saved sensitivity settings from PlayerPrefs
        KeyboardSensitivity = PlayerPrefs.GetFloat("KeyboardSensitivity", 1f);
        ControllerSensitivity = PlayerPrefs.GetFloat("ControllerSensitivity", 1f);
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        // Apply loaded values to sliders
        controllerSensitivitySlider.value = ControllerSensitivity;
        mouseSensitivitySlider.value = MouseSensitivity;

        // Update sensitivity UI visibility (logic currently commented out)
        UpdateSensitivityUI();
    }

    // Called when master volume slider changes
    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value); // Save new value
    }

    // Called when music volume slider changes
    public void OnMusicVolumeChanged(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;

        SoundManager.Instance.SetMusicVolume(value); // Update SoundManager
        PlayerPrefs.SetFloat("MusicVolume", value);  // Save new value
    }

    // Called when SFX volume slider changes
    public void OnSFXVolumeChanged(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;

        SoundManager.Instance.SetEffectsVolume(value); // Update SoundManager
        PlayerPrefs.SetFloat("SFXVolume", value);      // Save new value
    }

    // Applies current slider values to audio settings
    private void ApplyVolumeSettings()
    {
        OnMusicVolumeChanged(musicVolumeSlider.value);
        OnSFXVolumeChanged(sfxVolumeSlider.value);
        OnMasterVolumeChanged(masterVolumeSlider.value);
    }

    // Called when keyboard sensitivity slider changes
    public void OnKeyboardSensitivityChanged(float value)
    {
        KeyboardSensitivity = value;
        PlayerPrefs.SetFloat("KeyboardSensitivity", value); // Save new value
        /* if (currentInputMethod == InputMethod.Keyboard)
             ApplyCurrentSensitivity();*/ // Apply if keyboard is active (commented out)
    }

    // Called when controller sensitivity slider changes
    public void OnControllerSensitivityChanged(float value)
    {
        ControllerSensitivity = value;
        PlayerPrefs.SetFloat("ControllerSensitivity", value); // Save new value
        /* if (currentInputMethod == InputMethod.Controller)
              ApplyCurrentSensitivity();*/ // Apply if controller is active (commented out)
    }

    // Called when mouse sensitivity slider changes
    public void OnMouseSensitivityChanged(float value)
    {
        MouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value); // Save new value
        /*if (currentInputMethod == InputMethod.Mouse)
            ApplyCurrentSensitivity();*/ // Apply if mouse is active (commented out)
    }

    /* public void SetInputMethod(int methodIndex)
     {
         currentInputMethod = (InputMethod)methodIndex; // Set input method from dropdown index
         ApplyCurrentSensitivity();                     // Apply sensitivity for selected method
         UpdateSensitivityUI();                         // Update UI visibility
     }*/

    /*  void ApplyCurrentSensitivity()
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
      }*/

    // Updates which sensitivity slider is visible based on input method
    void UpdateSensitivityUI()
    {
        //keyboardSensitivitySlider.gameObject.SetActive(currentInputMethod == InputMethod.Keyboard);
        //controllerSensitivitySlider.gameObject.SetActive(currentInputMethod == InputMethod.Controller);
        //mouseSensitivitySlider.gameObject.SetActive(currentInputMethod == InputMethod.Mouse);
    }

    // Called by external input detection system to set input method
    public void SetInputMethodFromDevice(InputDeviceDetector.DeviceType deviceType)
    {
        /*  switch (deviceType)
          {
              case InputDeviceDetector.DeviceType.Keyboard:
                  currentInputMethod = InputMethod.Keyboard;
                  break;
              case InputDeviceDetector.DeviceType.Mouse:
                  currentInputMethod = InputMethod.Mouse;
                  break;
              case InputDeviceDetector.DeviceType.Gamepad:
                  currentInputMethod = InputMethod.Controller;
                  break;
          }*/

        //ApplyCurrentSensitivity(); // Apply sensitivity for detected device
        UpdateSensitivityUI();      // Update UI visibility
    }

    // Opens the settings panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Closes the settings panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}