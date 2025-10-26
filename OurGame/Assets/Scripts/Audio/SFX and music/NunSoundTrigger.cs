using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NunSoundTrigger : MonoBehaviour
{
    public AudioClip npcSound;           // Assign this in the Inspector
    public float triggerDistance = 5f;   // Distance threshold
    private Transform player;
    private AudioSource npcAudioSource;
    private bool hasPlayed = false;

    void Start()
    {
        // Get reference to player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure it's tagged 'Player'.");
        }

        // Get or configure local AudioSource
        npcAudioSource = GetComponent<AudioSource>();
        npcAudioSource.spatialBlend = 1f; // Enable 3D sound
        npcAudioSource.playOnAwake = false;
        npcAudioSource.loop = false;
    }

    void Update()
    {
        if (player == null || npcSound == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance && !hasPlayed)
        {
            npcAudioSource.clip = npcSound;
            npcAudioSource.Play();
            hasPlayed = true;
        }
        else if (distance > triggerDistance)
        {
            hasPlayed = false; // Reset so it can play again if player re-enters
        }
    }
}