using UnityEngine;

// Ensures the GameObject has an AudioSource component
[RequireComponent(typeof(AudioSource))]
public class NunSoundTrigger : MonoBehaviour
{
    public AudioClip npcSound;           // Sound clip to play when player is nearby
    public float triggerDistance = 5f;   // Distance threshold for triggering the sound

    private Transform player;            // Reference to the player's transform
    private AudioSource npcAudioSource;  // AudioSource component attached to this GameObject
    private bool hasPlayed = false;      // Flag to ensure sound plays only once per proximity entry

    void Start()
    {
        // Find the player GameObject by tag and store its transform
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            // Warn if player is not found
            Debug.LogWarning("Player not found. Make sure it's tagged 'Player'.");
        }

        // Get the AudioSource component and configure it for 3D playback
        npcAudioSource = GetComponent<AudioSource>();
        npcAudioSource.spatialBlend = 1f;     // Set to 3D sound
        npcAudioSource.playOnAwake = false;   // Prevent sound from playing on start
        npcAudioSource.loop = false;          // Play sound only once per trigger
    }

    void Update()
    {
        // Exit early if player or sound clip is missing
        if (player == null || npcSound == null) return;

        // Calculate distance between NPC and player
        float distance = Vector3.Distance(transform.position, player.position);

        // If player is within trigger distance and sound hasn't played yet
        if (distance <= triggerDistance && !hasPlayed && GameObject.FindObjectOfType<NunAi>().inLOS)
        {
            npcAudioSource.clip = npcSound;   // Assign the sound clip
            npcAudioSource.Play();            // Play the sound
            hasPlayed = true;                 // Mark as played to prevent repetition
        }
        else if (distance > triggerDistance)
        {
            // Reset flag when player moves out of range
            hasPlayed = false;
        }
    }
}