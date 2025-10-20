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
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask StopLayer;
    private MoveFromMicrophone microphoneInput;
    //Patrolling +  //Waypoints
    private ControllerRumble rumbler;
    private Interactor _interactor;
    private VignetteControl vignetteControl;
    //states
    public float sightRange, catchRange;
    public bool playerInSightRange, playerInCatchRange;
    bool isLoud;
    private bool onHiddenCooldownTime = false;


    public float NunlookTime = 5;
    private float _agentSpeed;
    public bool _isGracePeriod = true;

    private NunDoors nunDoors;
    private NunCatch nunCatch;
    private NunChase nunChase;
    private NunPatrol nunPatrol;


    public bool inLOS, isChasing;

    void Awake()
    {
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

        isLoud = microphoneInput.isLoud;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInCatchRange = Physics.CheckSphere(transform.position, catchRange, playerLayer);


        /*   Vector3 directionToPlayer = (player.position - transform.position).normalized;
           float distanceToPlayer = Vector3.Distance(transform.position, player.position);
          // playerinLOS = !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, StopLayer);

           Debug.DrawLine(transform.position, directionToPlayer);*/

        inLOS = true;
        isChasing = playerInSightRange && !playerInCatchRange && inLOS;

        if (_isGracePeriod == false)
        {
            // Rumble logic
            if (isLoud || isChasing)
            {
                rumbler.RumbleStream(0.2f, 0.5f, 0.25f);
            }
            else if (!isLoud && !isChasing)
            {
                rumbler.StopRumbleSteam();
            }


            if (!playerInSightRange && !playerInCatchRange) nunPatrol.Patrol();
            if ((isChasing || isLoud) && !onHiddenCooldownTime) nunChase.ChasePlayer();
            if (playerInSightRange && playerInCatchRange) nunCatch.CatchPlayer();
        }

        if (_interactor._PlayerIsHidden && !onHiddenCooldownTime)
            StartCoroutine(HiddenCooldown());
    }








    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.5f, 0.5f);
    }







    private IEnumerator HiddenCooldown()
    {
        float hiddenCooldownTime;
        onHiddenCooldownTime = true;
        hiddenCooldownTime = 5;
        Debug.Log("player is not chased for x seconds");
        while (hiddenCooldownTime > 0f)
        {
            nunPatrol.Patrol();
            hiddenCooldownTime -= Time.deltaTime;
            yield return null;
        }
        onHiddenCooldownTime = false;
    }


}
