using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorAudioTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip triggerSound;
    [Range(0f, 1f)] public float volume = 1.0f;

    private Animator animator;
    private AudioSource audioSource;
    private float lastNormalizedTime = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
        }
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Detect when animation restarts (normalizedTime wraps back to 0)
        if (stateInfo.normalizedTime < 0.1f && lastNormalizedTime > 0.9f)
        {
            if (triggerSound != null)
            {
                audioSource.PlayOneShot(triggerSound, volume);
            }
        }

        lastNormalizedTime = stateInfo.normalizedTime;
    }
}