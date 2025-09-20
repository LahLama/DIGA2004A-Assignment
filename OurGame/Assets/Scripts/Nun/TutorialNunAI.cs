using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TutorialNunAI : MonoBehaviour
{
    private NunAi nunBaseScript;
    private NavMeshAgent agent;
    private VignetteControl vignetteControl;
    private bool nunSpawned;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (TryGetComponent<NunAi>(out nunBaseScript))
        {
            nunBaseScript.enabled = false;
        }

        Invoke("SpawnNunOnPlayer", 5);
    }

    private void Update()
    {
        if (nunSpawned)
        {
            player.GetComponentInChildren<Camera>().transform.LookAt(new Vector3(agent.gameObject.transform.position.x, agent.gameObject.transform.position.y + (agent.height / 2), agent.gameObject.transform.position.z));
        }
    }

    public void SpawnNunOnPlayer()
    {
        vignetteControl.ApplyVignette(2);



        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;

        agent.gameObject.transform.position = player.transform.TransformPoint(new Vector3(0, 0, -3));
        nunSpawned = true;

        Invoke("MoveToPlayer", 2);
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(player.position);
    }

}
