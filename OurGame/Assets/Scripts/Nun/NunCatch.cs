using UnityEngine;
using UnityEngine.AI;
public class NunCatch : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    private GameObject lifeCounter;
    private Vector3 playerOGpos, nunOGpos;
    private NunPatrol nunPatrol;
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerOGpos = player.transform.position;
        nunOGpos = agent.gameObject.transform.position;
        lifeCounter = GameObject.FindWithTag("LifeTracker");
        nunPatrol = this.GetComponent<NunPatrol>();

    }
    public void CatchPlayer()
    {
        agent.SetDestination(transform.position);
        Vector3 lookPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + agent.height, this.gameObject.transform.position.z);
        player.GetComponentInChildren<Camera>().transform.LookAt(lookPos);
        agent.transform.LookAt(player.GetChild(0));
        Invoke("RespawnPlayer", 2f);


    }

    private void RespawnPlayer()
    {
        //Update the life count:
        bool HasRespawned = false;
        if (!HasRespawned)
        {
            //Reset the positons
            agent.Warp(nunOGpos);
            player.GetComponent<CharacterController>().enabled = false;
            player.position = playerOGpos;
            player.GetComponent<CharacterController>().enabled = true;
            nunPatrol.StartGracePeriod();

            lifeCounter.SendMessage("RecieveMessageCatchPlayer");
            HasRespawned = true;
        }
    }
}
