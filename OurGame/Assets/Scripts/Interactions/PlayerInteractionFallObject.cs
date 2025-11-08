using UnityEngine;

// This script triggers an animation and plays a sound when the player collides with the object.
// The sound is loaded from the Resources folder and played only once.
public class PlayerInteractionFallObject : MonoBehaviour
{
    public string fallSoundName = "Fall"; // Name of the sound clip located in the Resources folder
    private bool hasPlayedSound = false;  // Flag to ensure the sound plays only once

    // Called when another collider enters this object's trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player and the sound hasn't been played yet
        if (other.CompareTag("Player") && !hasPlayedSound)
        {
            // Trigger the interaction animation (assumes a component named 'interactionAnimation' is attached)
            GetComponent<interactionAnimation>().Interact();

            // Load the sound clip from the Resources folder
            AudioClip clip = Resources.Load<AudioClip>(fallSoundName);
            if (clip != null)
            {
                // Play the sound using a central SoundManager instance
                SoundManager.Instance.Play(clip);
                hasPlayedSound = true; // Prevent the sound from playing again
            }
            else
            {
                // Warn if the sound clip couldn't be found
                Debug.LogWarning($"Sound clip '{fallSoundName}' not found in Resources.");
            }
        }
    }
}