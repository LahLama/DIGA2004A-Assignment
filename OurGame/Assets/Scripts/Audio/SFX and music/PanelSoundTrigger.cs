using UnityEngine;
using UnityEngine.SceneManagement;

// This script manages background audio playback based on scene transitions.
// It ensures the audio persists across scenes and only plays in a designated scene.
public class PanelSoundTrigger : MonoBehaviour
{
    public AudioSource backgroundAudio;               // Reference to the AudioSource to control
    public string gameSceneName = "GameScene";        // Name of the scene where audio should play

    void Awake()
    {
        // Prevent the audio GameObject from being destroyed when loading a new scene
        if (backgroundAudio != null)
        {
            DontDestroyOnLoad(backgroundAudio.gameObject);
        }

        // Subscribe to the sceneLoaded event to respond to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        // Attempt to play audio when the object becomes active
        TryPlayAudio();
    }

    void OnDisable()
    {
        // Stop audio when the object is disabled
        TryStopAudio();
    }

    // Called automatically when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the loaded scene matches the target scene, play audio
        if (scene.name == gameSceneName)
        {
            TryPlayAudio();
        }
        else
        {
            // Otherwise, stop the audio
            TryStopAudio();
        }
    }

    // Plays the audio if it's not already playing and we're in the correct scene
    void TryPlayAudio()
    {
       
        if (backgroundAudio != null && !backgroundAudio.isPlaying && SceneManager.GetActiveScene().name == gameSceneName)
        {
            backgroundAudio.Play();
        }
    }

    // Stops the audio if it's currently playing
    void TryStopAudio()
    {
        if (backgroundAudio != null && backgroundAudio.isPlaying)
        {
            backgroundAudio.Stop();
        }
    }
}