using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NunPatrol : MonoBehaviour
{

    public Transform player;
    public NavMeshAgent agent;
    private float _agentSpeed;
    private NunDoors nunDoors;
    private VignetteControl vignetteControl;
    public List<Transform> waypoints;
    public int currentWayPointIndex = 0;
    bool isWaitingAtWaypoint = false;
    public bool _isGracePeriod = true;
    public float WaitPointDelay = 5;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _agentSpeed = agent.speed;
        nunDoors = this.GetComponent<NunDoors>();
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();

    }
    public void Patrol()
    {
        agent.speed = _agentSpeed;
        nunDoors.DoorInteractions();
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
    public void StartGracePeriod()
    {
        _isGracePeriod = true;
        Invoke("EndGracePeriod", 15);
    }

    private void EndGracePeriod()
    {
        _isGracePeriod = false;
    }
    IEnumerator WaitToGoToNextPoint(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentWayPointIndex++;
        if (currentWayPointIndex >= waypoints.Count)
            currentWayPointIndex = 0;
        isWaitingAtWaypoint = false;
    }
}
