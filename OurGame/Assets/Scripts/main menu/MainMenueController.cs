using UnityEngine;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    public GameObject clickEnterText;
    public GameObject videoRawImage;
    public VideoPlayer videoPlayer;

    public GameObject playButton;
    public GameObject creditButton;
    public GameObject quitButton;

    private bool hasStarted = false;

    void Start()
    {
        clickEnterText.SetActive(true);
        videoRawImage.SetActive(false);
        playButton.SetActive(false);
        creditButton.SetActive(false);
        quitButton.SetActive(false);
    }
     void Update()
    {
        if (!hasStarted && Input.GetMouseButtonDown(0))
        {
            hasStarted = true;

            clickEnterText.SetActive(false);
            videoRawImage.SetActive(true);

            if (videoPlayer != null)
            {
                videoPlayer.Play();
            }
            
            playButton.SetActive(true);
            creditButton.SetActive(true);
            quitButton.SetActive(true);
        }
    }
}
