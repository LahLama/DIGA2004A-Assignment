using UnityEngine;

// Ensures the GameObject has an Animator component
[RequireComponent(typeof(Animator))]
public class AnimatorAudioTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip triggerSound;                  // The sound to play when the animation restarts
    [Range(0f, 1f)] public float volume = 1.0f;     // Volume of the sound playback

    private Animator animator;                      // Reference to the Animator component
    private AudioSource audioSource;                // AudioSource used to play the sound
    private float lastNormalizedTime = 0f;          // Tracks the previous frame's normalized time

    void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Try to get an existing AudioSource component
        audioSource = GetComponent<AudioSource>();

        // If none exists, create and configure a new one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;        // Prevent sound from playing automatically on start
            audioSource.spatialBlend = 1f;          // Set to 3D sound (1 = fully 3D, 0 = 2D)
        }
    }

    void Update()
    {
        // Get the current animation state info from layer 0
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the animation has looped or restarted:
        // normalizedTime goes from 0 to 1 over the course of the animation
        // If it wraps from >0.9 back to <0.1, the animation has restarted
        if (stateInfo.normalizedTime < 0.1f && lastNormalizedTime > 0.9f)
        {
            // Play the trigger sound if it's assigned
            if (triggerSound != null)
            {
                audioSource.PlayOneShot(triggerSound, volume); // Play the sound once at specified volume
            }
        }

        // Update the last normalized time for the next frame comparison
        lastNormalizedTime = stateInfo.normalizedTime;
    }
}