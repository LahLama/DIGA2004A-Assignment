using UnityEngine;
using UnityEngine.AI;

public class TutorialNunAI : MonoBehaviour
{
    // References to other components and systems
    private NunPatrol nunPatrol;
    private NavMeshAgent agent;
    private VignetteControl vignetteControl;
    private bool nunSpawned = false;
    private Tutorial tutorial;
    private TutorialPickUp tutorialPickUp;
    private Transform player;
    public Vector3 OriginalPos;
    private Transform camera;

    // Tutorial and vignette states
    private bool TutEnded = false, TutItem = false, ApplyVignette = false;

    // Location where the nun will spawn near the player
    [SerializeField] private Transform NunSpawnPoint;

    void Start()
    {
        // Cache references to important systems and transforms
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        OriginalPos = transform.position;
        vignetteControl = FindAnyObjectByType<VignetteControl>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial = FindAnyObjectByType<Tutorial>();
        tutorialPickUp = tutorial.gameObject.transform.GetChild(0).GetComponent<TutorialPickUp>();
        nunPatrol = FindAnyObjectByType<NunPatrol>();

        // Reset any scaling issues that may arise
        this.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        // Behaviour when nun has spawned
        if (nunSpawned)
        {
            // Force the camera to look at the nun to create dramatic tension
            player.GetComponentInChildren<Camera>().transform.LookAt(
                new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + agent.height,
                    this.gameObject.transform.position.z
                )
            );

            // Make the nun face the player
            agent.transform.LookAt(GameObject.FindGameObjectWithTag("PlayerEyes").transform);

            // Apply vignette one time when nun appears
            if (ApplyVignette == false)
            {
                vignetteControl.ApplyVignette(1);
                ApplyVignette = true;
            }
        }

        // Check if the tutorial item has been collected
        TutItem = tutorialPickUp.hasBeenPicked;

        // Calculate distance between nun and player
        float dist = Vector3.Distance(player.position, agent.transform.position);

        // If item collected and nun has reached player, begin ending tutorial sequence
        if (TutItem && !TutEnded && dist <= agent.stoppingDistance)
        {
            Invoke("EndTut", 2); // Delay to allow black screen or animation
            TutEnded = true;
        }
    }

    private void EndTut()
    {
        // End tutorial actions
        tutorial.EndTutorial();
        vignetteControl.RemoveVignette(1);
        nunPatrol.StartGracePeriod();
        GetComponent<NunAi>()._isGracePeriod = true;
        player.GetComponent<Awakening>().enabled = true;
    }

    public void SpawnNunOnPlayer()
    {
        PlayerStats.Instance.playerLevel = PlayerStats.PlayerLevel.Cutscene;
        // Reset nun and player camera rotations
        this.transform.localScale = Vector3.one;
        camera.rotation = Quaternion.Euler(0, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        SoundManager.Instance.StopLooping("SprintStep");
        SoundManager.Instance.StopLooping("WalkStep");
        // Disable player movement to prevent interaction while nun spawns
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;

        // Move nun to defined spawn point instantly
        agent.Warp(NunSpawnPoint.position);

        nunSpawned = true;

        // After a short delay, start chasing player
        Invoke("MoveToPlayer", 2);
    }

    private void MoveToPlayer()
    {
        // Nun moves towards the player position
        agent.SetDestination(player.position);
    }
}
