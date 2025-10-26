using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NunChase : MonoBehaviour
{
    public NavMeshAgent agent;             // Agent used for navigation
    public Transform player;               // Player reference
    private VignetteControl vignetteControl; // Reference to vignette effect
    private float _agentSpeed;             // Store original agent speed
    private NunDoors nunDoors;             // Reference to door interactions
    public LayerMask StopLayer;            // Layer used for obstacles that block sight
    public float sightRange;               // Range at which nun can see the player

    public float NunlookTime = 2;          // Duration nun keeps chasing

    public bool los;                       // Stores whether player is in line of sight

    void Awake()
    {
        // Cache references
        sightRange = GetComponent<NunAi>().sightRange;
        vignetteControl = GameObject.FindAnyObjectByType<VignetteControl>();
        nunDoors = this.GetComponent<NunDoors>();
        _agentSpeed = agent.speed;
        player = GameObject.FindWithTag("Player").gameObject.transform;
    }

    public void ChasePlayer()
    {
        // Check if the player is on the correct layer and is in line of sight
        if (player.gameObject.layer == LayerMask.NameToLayer("Player") && PlayerInLineOfSight())
        {
            // Increase agent speed during chase
            agent.speed = _agentSpeed * (4 / 3.0f);

            // Interact with any doors in the way
            nunDoors.DoorInteractions();

            // Apply vignette effect to indicate tension
            vignetteControl.ApplyVignette(2);

            // Make agent face the player
            agent.transform.LookAt(player.GetChild(0));

            // Start chasing for a limited time
            StartCoroutine(ChaseTime(NunlookTime));
        }
        else
        {
            // Stop chasing if player not in sight
            StopCoroutine(ChaseTime(NunlookTime));
        }

        // Update line of sight flag
        los = PlayerInLineOfSight();
    }

    private IEnumerator ChaseTime(float delay)
    {
        float timer = delay;

        while (timer > 0f && (player.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            // Check if path to player is not blocked
            if (!Physics.Raycast(transform.position, transform.forward * sightRange, 1f, StopLayer))
            {
                agent.SetDestination(player.position); // Move towards player
            }

            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public bool PlayerInLineOfSight()
    {
        Vector3 directionToPlayer = player.position - agent.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Raycast from nun to player, considering obstacles
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, directionToPlayer.normalized, out hit, distanceToPlayer))
        {
            // Only return true if the first object hit is the player
            if (hit.transform == player)
            {
                return true;
            }
        }

        return false; // Player blocked or not hit
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a line to visualise line of sight in the editor
        if (agent != null && player != null)
        {
            Gizmos.color = Color.white;
            Vector3 directionToPlayer = player.position - agent.transform.position;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + directionToPlayer.normalized * Mathf.Min(sightRange, directionToPlayer.magnitude));
        }
    }
}
