using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSoundTrigger : MonoBehaviour
{
    public AudioSource backgroundAudio;
    public string gameSceneName = "GameScene"; // Replace with your actual game scene name

    void Awake()
    {
        if (backgroundAudio != null)
        {
            DontDestroyOnLoad(backgroundAudio.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        TryPlayAudio();
    }

    void OnDisable()
    {
        TryStopAudio();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            TryPlayAudio();
        }
        else
        {
            TryStopAudio();
        }
    }

    void TryPlayAudio()
    {
        if (backgroundAudio != null && !backgroundAudio.isPlaying && SceneManager.GetActiveScene().name == gameSceneName)
        {
            backgroundAudio.Play();
        }
    }

    void TryStopAudio()
    {
        if (backgroundAudio != null && backgroundAudio.isPlaying)
        {
            backgroundAudio.Stop();
        }
    }
}