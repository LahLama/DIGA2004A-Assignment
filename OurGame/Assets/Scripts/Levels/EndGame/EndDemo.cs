using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndDemo : MonoBehaviour, IInteractables
{
    [Header("Video Settings")]
    public GameObject videoRawImage;
    public VideoPlayer videoPlayer;
    public GameObject UIPanel;

    [Header("Audio Settings")]
    public AudioSource musicSource;
    public double triggerTime = 10.0; // Time in seconds when music should start

    [Header("Scene Settings")]
    public string nextSceneName = "final cutscene "; // Set this in Inspector

    private bool hasTriggeredAudio = false;
    private bool hasPlayedVideo = false;

    public void Interact()
    {
        if (!hasPlayedVideo)
        {
            EndDemoScreen();
        }
    }

    private void EndDemoScreen()
    {
        Debug.Log("Transition to the END VIDEO here");

        videoRawImage.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            hasTriggeredAudio = false;
            hasPlayedVideo = true;

            if (musicSource != null)
                musicSource.Stop(); // Reset music

            videoPlayer.loopPointReached += OnVideoFinished; // Subscribe to end event
        }

        UIPanel.SetActive(false);
    }

    private void Update()
    {
        if (videoPlayer != null && videoPlayer.isPlaying && !hasTriggeredAudio)
        {
            if (videoPlayer.time >= triggerTime)
            {
                if (musicSource != null)
                    musicSource.Play();

                hasTriggeredAudio = true;
            }
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished. Loading next scene...");
        SceneManager.LoadScene(nextSceneName);
    }
}
