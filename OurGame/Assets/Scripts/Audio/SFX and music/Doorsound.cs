using UnityEngine;

// This script plays a sound when the door opens, based on the animation state.
public class DoorSoundTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;     // The AudioSource component that will play the sound
    public AudioClip openSound;         // The sound clip to play when the door opens

    [Header("Animator")]
    public Animator doorAnimator;       // Reference to the Animator controlling the door
    public string openTriggerName = "Open"; // Name of the trigger parameter used to open the door

    private bool hasPlayedSound = false; // Flag to ensure the sound plays only once per open animation

    void Update()
    {
        // Check if the current animation state is "DoorOpen"
        // Replace "DoorOpen" with the actual name of your door's open animation state
        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen"))
        {
            // If the sound hasn't been played yet during this animation cycle
            if (!hasPlayedSound)
            {
                PlayOpenSound();        // Play the door opening sound
                hasPlayedSound = true; // Mark that the sound has been played
            }
        }
        else
        {
            // Reset the flag when the door is not in the open state
            // This allows the sound to play again the next time the door opens
            hasPlayedSound = false;
        }
    }

    // Plays the door opening sound using the AudioSource
    void PlayOpenSound()
    {
        // Ensure both the AudioSource and AudioClip are assigned before playing
        if (audioSource && openSound)
        {
            audioSource.PlayOneShot(openSound); // Play the sound once without interrupting other audio
        }
    }
}