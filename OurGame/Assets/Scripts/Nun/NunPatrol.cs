using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NunPatrol : MonoBehaviour
{
    public Transform player;                // Player reference for potential tracking behaviour
    public NavMeshAgent agent;              // NavMeshAgent used to move the nun along the NavMesh

    private float _agentSpeed;              // Stored speed so speed can be restored after modifications
    private NunDoors nunDoors;              // Reference to script that handles door interactions
    private VignetteControl vignetteControl;// Reference to the vignette visual effect

    public List<Transform> waypoints;       // Patrol route waypoints
    public int currentWayPointIndex = 0;    // Tracks which waypoint is the next destination
    bool isWaitingAtWaypoint = false;       // Patrolling pause state

    public bool _isGracePeriod = true;      // Grace period means nun does not chase the player yet
    public float WaitPointDelay = 5;        // Time spent at each waypoint before moving on

    void Awake()
    {
        // Cache references and starting values
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _agentSpeed = agent.speed;
        nunDoors = this.GetComponent<NunDoors>();
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
    }

    public void Patrol()
    {
        // Reset speed and allow interactions with doors
        agent.speed = _agentSpeed;
        nunDoors.DoorInteractions();

        // Remove vignette during patrolling to indicate normal state
        vignetteControl.RemoveVignette(2);

        float distanceToWayPoint = 0f;
        distanceToWayPoint = Vector3.Distance(waypoints[currentWayPointIndex].position, transform.position);

        // When the waypoint is reached, wait before switching target
        if (distanceToWayPoint <= agent.stoppingDistance)
        {
            if (!isWaitingAtWaypoint)
            {
                isWaitingAtWaypoint = true;
                StartCoroutine(WaitToGoToNextPoint(WaitPointDelay));
            }
            return;
        }

        // Continue moving towards the next patrol point
        agent.SetDestination(waypoints[currentWayPointIndex].position);
    }

    public void StartGracePeriod()
    {
        // Temporary calm period before the nun can chase
        _isGracePeriod = true;
        Invoke("EndGracePeriod", 15); // Automatically end after a delay
    }

    private void EndGracePeriod()
    {
        // Nun becomes active and dangerous once more
        _isGracePeriod = false;
    }

    IEnumerator WaitToGoToNextPoint(float delay)
    {
        // Delay movement to simulate looking around or waiting
        yield return new WaitForSeconds(delay);

        currentWayPointIndex++;

        // Loop back to first waypoint when the final one is reached
        if (currentWayPointIndex >= waypoints.Count)
            currentWayPointIndex = 0;

        isWaitingAtWaypoint = false;
    }
}
