using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//https://www.youtube.com/watch?v=vS6lyX2QidE
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask StopLayer;


    //Patrolling
    public int currentWayPointIndex = 0;
    //Waypoints
    public List<Transform> waypoints;





    //states
    public float sightRange, catchRange;
    public bool playerInSightRange, playerInCatchRange;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInCatchRange = Physics.CheckSphere(transform.position, catchRange, playerLayer);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10f, Color.magenta);

        if (!playerInSightRange && !playerInCatchRange) Patrol();
        if (playerInSightRange && !playerInCatchRange) ChasePlayer();
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
        float distanceToWayPoint = 0f;
        if (currentWayPointIndex <= waypoints.Count - 1)
            distanceToWayPoint = Vector3.Distance(waypoints[currentWayPointIndex].position, transform.position);
        else
        {
            currentWayPointIndex = 0;
            return;
        }


        if (distanceToWayPoint <= 1)
        {
            currentWayPointIndex++;
            return;
        }

        agent.SetDestination(waypoints[currentWayPointIndex].position);

        DoorInteractions();
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
            StartCoroutine(ActivateDoorAfterDelay(hitObj, 1.5f));
        }

    }

    IEnumerator ActivateDoorAfterDelay(GameObject hitObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitObj.SetActive(true);
    }


    private void CatchPlayer()
    {
        agent.SetDestination(player.position);

        Debug.Log("CAUGHT THE PLAYER");

    }
    private void ChasePlayer()
    {
        //stops the agent
        agent.SetDestination(player.position);

        //looks at player
        transform.LookAt(player);

    }

}
