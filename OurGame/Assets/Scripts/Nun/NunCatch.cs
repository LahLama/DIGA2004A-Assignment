using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NunCatch : MonoBehaviour
{
    public NavMeshAgent agent;             // NavMeshAgent used for movement
    public Transform player;               // Player reference
    private LivesTracker lifeCounter;      // Tracks player lives
    private Vector3 playerOGpos, nunOGpos; // Store original positions for potential respawn
    private NunPatrol nunPatrol;           // Reference to patrol behaviour
    bool HasRespawned = false;             // Flag to prevent multiple respawns

    void Awake()
    {
        // Cache references and initial positions
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerOGpos = player.transform.position;
        nunOGpos = agent.gameObject.transform.position;
        lifeCounter = GameObject.FindWithTag("PlayerStats").GetComponent<LivesTracker>();
        nunPatrol = this.GetComponent<NunPatrol>();
    }

    public void CatchPlayer()
    {
        SoundManager.Instance.StopLooping("SprintStep");
        SoundManager.Instance.StopLooping("WalkStep");
        // Stop the nun in place
        agent.SetDestination(transform.position);

        // Make the player's camera look at the nun
        Vector3 lookPos = new Vector3(
            this.gameObject.transform.position.x,
            this.gameObject.transform.position.y + agent.height,
            this.gameObject.transform.position.z
        );
        player.GetComponentInChildren<Camera>().transform.LookAt(lookPos);

        // Make the nun face the player
        agent.transform.LookAt(GameObject.FindGameObjectWithTag("PlayerEyes").transform);

        /* GAME OVER LOGIC */
        Invoke("endGameOnCatch", 4f);

        player.GetComponent<LookFunction>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;


        /* 
        Optional respawn logic (commented out)
        HasRespawned = false;
        Invoke("RespawnPlayer", 2f);
        GetComponent<NunPatrol>()._isGracePeriod = true;
        player.GetComponent<Awakening>().enabled = true;
        */

        return;
    }

    /*
    private void RespawnPlayer()
    {
        // Update life count and reset positions if not already respawned
        if (!HasRespawned)
        {
            HasRespawned = true;

            // Reset nun and player positions
            agent.Warp(nunOGpos);
            player.GetComponent<CharacterController>().enabled = false;
            player.position = playerOGpos;
            player.GetComponent<CharacterController>().enabled = true;

            // Start grace period to prevent immediate chase
            nunPatrol.StartGracePeriod();

            // Notify life counter that player was caught
            lifeCounter.SendMessage("RecieveMessageCatchPlayer");
        }
    }
    */

    void endGameOnCatch()
    {
        // Log and load end scene
        Debug.Log("GAME IS NOW OVER");
        SceneManager.LoadScene("DeathScene");
    }
}
