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
    public LayerMask DoorLayer;
    public LayerMask StopLayer;
    private MoveFromMicrophone microphoneInput;
    //Patrolling +  //Waypoints
    public int currentWayPointIndex = 0;
    public List<Transform> waypoints;


    private VignetteControl vignetteControl;

    private ControllerRumble rumbler;

    //states
    public float sightRange, catchRange;
    public bool playerInSightRange, playerInCatchRange, playerinLOS;
    bool isWaitingAtWaypoint = false, isLoud;
    RaycastHit isPlayer;

    public float WaitPointDelay = 5;
    public float NunlookTime = 6;
    private float _agentSpeed;
    private float _gracePeriod = 15;
    private bool _isGracePeriod = false;
    private Vector3 playerOGpos, nunOGpos;


    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        microphoneInput = GameObject.Find("Microphone").GetComponent<MoveFromMicrophone>();
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
        rumbler = GameObject.FindGameObjectWithTag("ControllerManager").GetComponent<ControllerRumble>();
        _agentSpeed = agent.speed;

        playerOGpos = player.transform.position;
        nunOGpos = agent.gameObject.transform.position;

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

        bool isChasing = (playerInSightRange && !playerInCatchRange);

        if (_isGracePeriod)
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

            if (!playerInSightRange && !playerInCatchRange) Patrol();
            if (isChasing || microphoneInput.isLoud) ChasePlayer();
            if (playerInSightRange && playerInCatchRange) CatchPlayer();
        }



    }

    public void StartGracePeriod()
    {
        Invoke("EndGracePeriod", 15);
    }

    private void EndGracePeriod()
    {
        _isGracePeriod = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void Patrol()
    {
        agent.speed = _agentSpeed;
        DoorInteractions();
        vignetteControl.RemoveVignette(2);


        float distanceToWayPoint = 0f;

        distanceToWayPoint = Vector3.Distance(waypoints[currentWayPointIndex].position, transform.position);



        if (distanceToWayPoint <= agent.stoppingDistance)
        {
            if (!isWaitingAtWaypoint)
            {
                isWaitingAtWaypoint = true;
                StartCoroutine(WaitToGoToNextPoint(WaitPointDelay));
            }
            return;
        }

        agent.SetDestination(waypoints[currentWayPointIndex].position);




    }

    private void DoorInteractions()
    {

        if (Physics.Raycast(transform.position, transform.forward, 1f, DoorLayer))
        {
            RaycastHit hit;
            GameObject hitObj = null;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, DoorLayer))
                hitObj = hit.collider.gameObject;

            hitObj.SetActive(false);
            StartCoroutine(ActivateDoorAfterDelay(hitObj, 2f));
        }

    }

    IEnumerator ActivateDoorAfterDelay(GameObject hitObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitObj.SetActive(true);
    }
    IEnumerator WaitToGoToNextPoint(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentWayPointIndex++;
        if (currentWayPointIndex >= waypoints.Count)
            currentWayPointIndex = 0;
        isWaitingAtWaypoint = false;
    }


    private void CatchPlayer()
    {
        agent.SetDestination(transform.position);

        agent.Warp(nunOGpos);
        player.position = playerOGpos;

        Debug.Log("CAUGHT THE PLAYER");

    }
    private void ChasePlayer()
    {
        agent.speed = _agentSpeed * (5 / 3.0f);
        DoorInteractions();
        vignetteControl.ApplyVignette(2);
        agent.transform.LookAt(player.GetChild(0));

        StartCoroutine(ChaseTime(NunlookTime));


        //looks at player
        //transform.LookAt(player);


    }

    private IEnumerator ChaseTime(float delay)
    {
        float timer = delay;
        while (timer > 0f)
        {
            if (!Physics.Raycast(transform.position, transform.forward * sightRange, 1f, StopLayer))
            {
                //stops the agent
                agent.SetDestination(player.position);
            }
            else
            {
                agent.SetDestination(transform.position);

            }
            timer -= Time.deltaTime;
            yield return null;
        }
    }


}
