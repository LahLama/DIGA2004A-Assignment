using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    Title: How to make Navmesh AI move between different waypoints - Unity 3D 
    Author: LearnWithYas
    Date:  Oct 3, 2023
    Availability: https://www.youtube.com/watch?v=vS6lyX2QidE
*/

public class NunAi : MonoBehaviour
{
    public NavMeshAgent agent;                 // Agent used for movement
    public Transform player;                   // Player reference
    public LayerMask playerLayer;              // Layer mask for detecting player
    public LayerMask StopLayer;                // Layer mask for obstacles
    private MoveFromMicrophone microphoneInput;// Detects player sound levels
    private ControllerRumble rumbler;          // Handles controller vibrations
    private Interactor _interactor;            // Tracks player hiding state
    private VignetteControl vignetteControl;   // Controls visual vignette effects


    // Detection and range variables
    public float sightRange, catchRange;       // Ranges for sight and catching
    public bool playerInSightRange, playerInCatchRange; // Booleans tracking if player is detected
    bool isLoud;                               // Is player making loud noises
    private bool onHiddenCooldownTime = false; // Cooldown when player is hidden

    public float NunlookTime = 5;              // Duration for looking at player
    private float _agentSpeed;                 // Original agent speed
    public bool _isGracePeriod = true;         // Grace period where nun does not chase
    public float graceDelay = 25;

    private NunDoors nunDoors;                 // Handles door interactions
    private NunCatch nunCatch;                 // Handles catching player
    private NunChase nunChase;                 // Handles chasing player
    private NunPatrol nunPatrol;               // Handles patrolling behaviour

    public bool inLOS, isChasing;              // Line-of-sight and chasing flags

    void Awake()
    {
        // Cache references
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        microphoneInput = GameObject.FindAnyObjectByType<MoveFromMicrophone>();
        vignetteControl = GameObject.FindAnyObjectByType<VignetteControl>();
        rumbler = GameObject.FindAnyObjectByType<ControllerRumble>();
        _agentSpeed = agent.speed;
        _interactor = GameObject.FindAnyObjectByType<Interactor>();

        nunDoors = this.GetComponent<NunDoors>();
        nunCatch = this.GetComponent<NunCatch>();
        nunChase = this.GetComponent<NunChase>();
        nunPatrol = this.GetComponent<NunPatrol>();
    }

    void Update()
    {
        // Update noise and detection states
        isLoud = microphoneInput.isLoud;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInCatchRange = Physics.CheckSphere(transform.position, catchRange, playerLayer);

        // Currently assumes line of sight is always true
        inLOS = PlayerInLineOfSight();
        isChasing = playerInSightRange && !playerInCatchRange && inLOS || isLoud;

        // Only act if grace period has ended
        if (_isGracePeriod == false)
        {
            // Handle controller rumble
            if (isChasing)
            {
                rumbler.RumbleStream(0.2f, 0.5f, 0.25f);
                FindAnyObjectByType<NunSoundTrigger>().PlayNunMusic();
            }
            else if (!isChasing)
            {
                rumbler.StopRumbleSteam();
            }

            // Patrolling, chasing, and catching logic
            if (!playerInSightRange && !playerInCatchRange) nunPatrol.Patrol();
            if ((isChasing) && !onHiddenCooldownTime) nunChase.ChasePlayer();
            if (playerInSightRange && playerInCatchRange && inLOS) nunCatch.CatchPlayer();
        }

        // Handle player hiding cooldown
        if (_interactor._PlayerIsHidden && !onHiddenCooldownTime)
            StartCoroutine(HiddenCooldown());
    }

    private void OnDrawGizmos()
    {
        // Visualise ranges in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, catchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.5f, 0.5f);*/
    }

    private IEnumerator HiddenCooldown()
    {
        float hiddenCooldownTime;
        onHiddenCooldownTime = true; // Set cooldown active
        hiddenCooldownTime = 5;      // Duration of cooldown
        Debug.Log("player is not chased for x seconds");

        while (hiddenCooldownTime > 0f)
        {
            nunPatrol.Patrol();       // Keep patrolling while player hidden
            hiddenCooldownTime -= Time.deltaTime;
            yield return null;
        }

        onHiddenCooldownTime = false; // Reset cooldown flag
    }

    public void StartGracePeriod()
    {
        // Temporary calm period before the nun can chase
        _isGracePeriod = true;
        Invoke("EndGracePeriod", graceDelay); // Automatically end after a delay
    }

    private void EndGracePeriod()
    {
        // Nun becomes active and dangerous once more
        _isGracePeriod = false;
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
}
