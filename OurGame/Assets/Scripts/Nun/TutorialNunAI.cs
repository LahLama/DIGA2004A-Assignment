using UnityEngine;
using UnityEngine.AI;

public class TutorialNunAI : MonoBehaviour
{
    private NunAi nunBaseScript;
    private NavMeshAgent agent;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (TryGetComponent<NunAi>(out nunBaseScript))
        {
            nunBaseScript.enabled = false;
        }

        Invoke("SpawnNunOnPlayer", 5);
    }

    public void SpawnNunOnPlayer()
    {

        agent.gameObject.transform.position = player.transform.TransformPoint(new Vector3(0, 0, -5)); ;
    }

}
