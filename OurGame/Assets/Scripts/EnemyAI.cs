using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//https://www.youtube.com/watch?v=vS6lyX2QidE
//https://discussions.unity.com/t/how-to-change-the-color-of-the-vignette-through-script/326332
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask StopLayer;
    private MoveFromMicrophone microphoneInput;
    //Patrolling +  //Waypoints
    public int currentWayPointIndex = 0;
    public List<Transform> waypoints;


    private VignetteControl vignetteControl;

    //states
    public float sightRange, catchRange;
    public bool playerInSightRange, playerInCatchRange, playerinLOS;
    bool isWaitingAtWaypoint = false, isLoud;
    RaycastHit isPlayer;
    public float WaitPointDelay = 5;
    public float NunlookTime = 6;


    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        microphoneInput = GameObject.Find("Microphone").GetComponent<MoveFromMicrophone>();
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
    }

    void Update()
    {
        isLoud = microphoneInput.isLoud;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInCatchRange = Physics.CheckSphere(transform.position, catchRange, playerLayer);
        playerinLOS = Physics.Raycast(transform.position, transform.forward, out isPlayer, sightRange * 2, playerLayer);


        Debug.DrawRay(transform.position, transform.forward * sightRange * 2, Color.magenta);

        if (!playerInSightRange && !playerInCatchRange) Patrol();
        if ((playerInSightRange || playerinLOS && !playerInCatchRange) || (microphoneInput.isLoud)) ChasePlayer();
        if (playerInSightRange && playerInCatchRange) CatchPlayer();




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

        DoorInteractions();
        vignetteControl.RemoveVignette(2);


        float distanceToWayPoint = 0f;

        distanceToWayPoint = Vector3.Distance(waypoints[currentWayPointIndex].position, transform.position);



        if (distanceToWayPoint <= 1)
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

        if (Physics.Raycast(transform.position, transform.forward, 1f, StopLayer))
        {
            RaycastHit hit;
            GameObject hitObj = null;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, StopLayer))
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

        Debug.Log("CAUGHT THE PLAYER");

    }
    private void ChasePlayer()
    {

        DoorInteractions();
        vignetteControl.ApplyVignette(2);
        //stops the agent
        agent.SetDestination(transform.position);
        agent.SetDestination(player.position);

        //looks at player
        //transform.LookAt(player);


    }

}
