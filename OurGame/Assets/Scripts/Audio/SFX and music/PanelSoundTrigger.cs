using UnityEngine;

public class PanelSoundTrigger : MonoBehaviour
{
    public AudioSource backgroundAudio;

    void OnEnable()
    {
        if (backgroundAudio != null && !backgroundAudio.isPlaying)
            backgroundAudio.Play();
    }

    void OnDisable()
    {
        if (backgroundAudio != null && backgroundAudio.isPlaying)
            backgroundAudio.Stop();
    }
}