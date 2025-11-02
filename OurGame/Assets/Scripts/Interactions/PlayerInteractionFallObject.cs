using UnityEngine;

public class PlayerInteractionFallObject : MonoBehaviour
{
    public string fallSoundName = "Fall"; // Name of the sound clip in Resources
    private bool hasPlayedSound = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayedSound)
        {
            // Trigger animation
            GetComponent<interactionAnimation>().Interact();

            // Play sound once
            AudioClip clip = Resources.Load<AudioClip>(fallSoundName);
            if (clip != null)
            {
                SoundManager.Instance.Play(clip);
                hasPlayedSound = true;
            }
            else
            {
                Debug.LogWarning($"Sound clip '{fallSoundName}' not found in Resources.");
            }
        }
    }
}
