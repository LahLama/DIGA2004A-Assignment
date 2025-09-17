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
    //Patrolling
    public int currentWayPointIndex = 0;
    //Waypoints
    public List<Transform> waypoints;
    public Material fullscreenEffectMaterial;

    private float currentIntensity = 0f;
    private float targetIntensity = 0f;
    public float lerpSpeed = 2f; // Adjust for faster/slower transitions



    //states
    public float sightRange, catchRange;
    public bool playerInSightRange, playerInCatchRange, playerinLOS;
    bool isWaitingAtWaypoint = false;
    RaycastHit isPlayer;


    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        microphoneInput = GameObject.Find("Microphone").GetComponent<MoveFromMicrophone>();
    }

    void Update()
    {
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

        ///UN Dramatic Mode
       //Dramatic Mode

        targetIntensity = 0f;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);
        ///

        agent.SetDestination(waypoints[currentWayPointIndex].position);
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
            if (!isWaitingAtWaypoint)
            {
                isWaitingAtWaypoint = true;
                StartCoroutine(WaitToGoToNextPoint(2f));
            }
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
    IEnumerator WaitToGoToNextPoint(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentWayPointIndex++;
        isWaitingAtWaypoint = false;
    }


    private void CatchPlayer()
    {
        agent.SetDestination(transform.position);

        Debug.Log("CAUGHT THE PLAYER");

    }
    private void ChasePlayer()
    {
        //stops the agent
        agent.SetDestination(transform.position);
        agent.SetDestination(player.position);

        //looks at player
        transform.LookAt(player);

        //Dramatic Mode

        targetIntensity = 1f;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lerpSpeed);
        fullscreenEffectMaterial.SetFloat("_FullscreenIntensity", currentIntensity);


    }

}
