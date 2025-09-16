using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask ground;
    public LayerMask playerLayer;
    public LayerMask StopLayer;


    //Patrolling
    public Vector3 walkPoint;
    private bool _walkPointSet;
    public float walkPointRange;




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
        if (!_walkPointSet) SearchForWalkPoint();

        if (_walkPointSet)

            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 5)
            _walkPointSet = false;




    }

    private void SearchForWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, +walkPointRange);
        float randomX = Random.Range(-walkPointRange, +walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);



        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && !Physics.Raycast(transform.position, transform.forward, 10f, StopLayer))
            _walkPointSet = true;

        else
            _walkPointSet = false;

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
