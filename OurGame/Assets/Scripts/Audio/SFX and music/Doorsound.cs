using UnityEngine;

public class DoorSoundTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;

    [Header("Animator")]
    public Animator doorAnimator;
    public string openTriggerName = "Open"; // Adjust to match your Animator parameter

    private bool hasPlayedSound = false;

    void Update()
    {
        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen")) // Replace with your animation state name
        {
            if (!hasPlayedSound)
            {
                PlayOpenSound();
                hasPlayedSound = true;
            }
        }
        else
        {
            hasPlayedSound = false; // Reset when not in open state
        }
    }

    void PlayOpenSound()
    {
        if (audioSource && openSound)
        {
            audioSource.PlayOneShot(openSound);
        }
    }
}