using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Interactor _interactor;

    //states
    public float sightRange, catchRange;
    public bool playerInSightRange, playerInCatchRange, playerinLOS;
    bool isWaitingAtWaypoint = false, isLoud;
    RaycastHit isPlayer;
    private bool onHiddenCooldownTime = false;
    private float hiddenCooldownTime;

    public float WaitPointDelay = 5;
    public float NunlookTime = 2;
    private float _agentSpeed;
    public bool _isGracePeriod = true;
    private Vector3 playerOGpos, nunOGpos;
    private GameObject lifeCounter;


    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        microphoneInput = GameObject.Find("Microphone").GetComponent<MoveFromMicrophone>();
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
        rumbler = GameObject.FindGameObjectWithTag("ControllerManager").GetComponent<ControllerRumble>();
        _agentSpeed = agent.speed;
        _interactor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Interactor>();
        lifeCounter = GameObject.FindWithTag("LifeTracker");
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

            if (!playerInSightRange && !playerInCatchRange) Patrol();
            if (isChasing || isLoud && !onHiddenCooldownTime) ChasePlayer();
            if (playerInSightRange && playerInCatchRange) CatchPlayer();
        }

        if (_interactor._PlayerIsHidden && !onHiddenCooldownTime)
            StartCoroutine(HiddenCooldown());
    }

    public void StartGracePeriod()
    {
        _isGracePeriod = true;
        Invoke("EndGracePeriod", 15);
    }

    private void EndGracePeriod()
    {
        _isGracePeriod = false;
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
        Vector3 sphereCenter = transform.position + transform.forward * 0.5f;
        float radius = 0.5f;
        Collider[] hits = Physics.OverlapSphere(sphereCenter, radius, DoorLayer);
        foreach (Collider col in hits)
        {
            GameObject hitObj = col.gameObject;
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
        Vector3 lookPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + agent.height, this.gameObject.transform.position.z);
        player.GetComponentInChildren<Camera>().transform.LookAt(lookPos);
        agent.transform.LookAt(player.GetChild(0));
        Invoke("RespawnPlayer", 2f);


    }
    private void ChasePlayer()
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            agent.speed = _agentSpeed * (5 / 3.0f);
            DoorInteractions();
            vignetteControl.ApplyVignette(2);
            agent.transform.LookAt(player.GetChild(0));

            StartCoroutine(ChaseTime(NunlookTime));
            //looks at player
            //transform.LookAt(player);
        }
        else
            StopCoroutine(ChaseTime(NunlookTime));
    }

    private void RespawnPlayer()
    {
        //Update the life count:
        bool HasRespawned = false;
        //Reset the positons
        agent.Warp(nunOGpos);
        player.GetComponent<CharacterController>().enabled = false;
        player.position = playerOGpos;
        player.GetComponent<CharacterController>().enabled = true;
        StartGracePeriod();
        if (!HasRespawned)
        {
            lifeCounter.SendMessage("RecieveMessageCatchPlayer");
            HasRespawned = true;
        }


    }

    private IEnumerator ChaseTime(float delay)
    {
        float timer = delay;
        while (timer > 0f && (player.gameObject.layer == LayerMask.NameToLayer("Player")))
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

    private IEnumerator HiddenCooldown()
    {

        onHiddenCooldownTime = true;
        hiddenCooldownTime = 5;
        Debug.Log("player is not chased for x seconds");
        while (hiddenCooldownTime > 0f)
        {
            Patrol();
            hiddenCooldownTime -= Time.deltaTime;
            yield return null;
        }
        onHiddenCooldownTime = false;





    }


}
