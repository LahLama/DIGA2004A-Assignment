using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioClip[] soundEffects;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PlaySound(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
            audioSource.PlayOneShot(soundEffects[index]);
    }
}
