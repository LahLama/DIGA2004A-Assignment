using UnityEngine;

public class PlayerInteractionFallObject : MonoBehaviour
{
    public string fallSoundName = "Fall"; // Name of the sound clip in Resources

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger animation
            GetComponent<interactionAnimation>().Interact();

            // Play sound
            AudioClip clip = Resources.Load<AudioClip>(fallSoundName);
            if (clip != null)
            {
                SoundManager.Instance.Play(clip);
            }
            else
            {
                Debug.LogWarning($"Sound clip '{fallSoundName}' not found in Resources.");
            }
        }
    }

    void Update()
    {
        
    }
}
