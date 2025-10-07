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
    void Awake()
    {

        vignetteControl = GameObject.FindAnyObjectByType<VignetteControl>();
        nunDoors = this.GetComponent<NunDoors>();


    }
    public void ChasePlayer()
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            agent.speed = _agentSpeed * (5 / 3.0f);
            nunDoors.DoorInteractions();
            vignetteControl.ApplyVignette(2);
            agent.transform.LookAt(player.GetChild(0));

            StartCoroutine(ChaseTime(NunlookTime));
            //looks at player
            //transform.LookAt(player);
        }
        else
            StopCoroutine(ChaseTime(NunlookTime));
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
}
