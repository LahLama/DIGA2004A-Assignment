using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NunChase : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    private VignetteControl vignetteControl;
    private float _agentSpeed;
    private NunDoors nunDoors;
    public LayerMask StopLayer;
    public float sightRange;

    public float NunlookTime = 2;

    public bool los;
    void Awake()
    {
        sightRange = GetComponent<NunAi>().sightRange;
        vignetteControl = GameObject.FindAnyObjectByType<VignetteControl>();
        nunDoors = this.GetComponent<NunDoors>();

        _agentSpeed = agent.speed;
    }
    public void ChasePlayer()
    {

        if (player.gameObject.layer == LayerMask.NameToLayer("Player") && PlayerInLineOfSight())
        {
            agent.speed = _agentSpeed * (4 / 3.0f);
            nunDoors.DoorInteractions();
            vignetteControl.ApplyVignette(2);
            agent.transform.LookAt(player.GetChild(0));

            StartCoroutine(ChaseTime(NunlookTime));
            //looks at player
            //transform.LookAt(player);
        }
        else
            StopCoroutine(ChaseTime(NunlookTime));

        los = PlayerInLineOfSight();
    }


    private IEnumerator ChaseTime(float delay)
    {
        float timer = delay;
        while (timer > 0f && (player.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            if (!Physics.Raycast(transform.position, transform.forward * sightRange, 1f, StopLayer))
            {
                agent.SetDestination(player.position);
            }
            else
            {
            }
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public bool PlayerInLineOfSight()
    {
        Vector3 directionToPlayer = player.position - agent.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Raycast from nun to player, using StopLayer as the mask for obstacles
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, directionToPlayer.normalized, out hit, distanceToPlayer))
        {


            // Only true if the first thing hit is the player
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {

        if (agent != null && player != null)
        {
            Gizmos.color = Color.red;
            Vector3 directionToPlayer = player.position - agent.transform.position;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + directionToPlayer.normalized * Mathf.Min(sightRange, directionToPlayer.magnitude));
        }
    }
}
