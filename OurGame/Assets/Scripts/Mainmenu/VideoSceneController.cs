using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneController : MonoBehaviour
{
    [Header("Video Settings")]
    public GameObject videoRawImage;
    public VideoPlayer videoPlayer;

    [Header("Audio Settings")]
    public AudioSource musicSource;
    public double triggerTime = 10.0;

    [Header("Scene Settings")]
    public string nextSceneName = "CreditsScene"; // Set this to your final scene

    private bool hasTriggeredAudio = false;

    void Start()
    {
        if (videoRawImage != null)
            videoRawImage.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoFinished;

            if (musicSource != null)
                musicSource.Stop();
        }
    }

    void Update()
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
        Debug.Log("Video finished. Loading final scene...");
        SceneManager.LoadScene(nextSceneName);
    }
}