using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class NunCatch : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    private LivesTracker lifeCounter;
    private Vector3 playerOGpos, nunOGpos;
    private NunPatrol nunPatrol;
    bool HasRespawned = false;
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerOGpos = player.transform.position;
        nunOGpos = agent.gameObject.transform.position;
        lifeCounter = GameObject.FindWithTag("PlayerStats").GetComponent<LivesTracker>();
        nunPatrol = this.GetComponent<NunPatrol>();

    }
    public void CatchPlayer()
    {
        agent.SetDestination(transform.position);
        Vector3 lookPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + agent.height, this.gameObject.transform.position.z);
        player.GetComponentInChildren<Camera>().transform.LookAt(lookPos);
        agent.transform.LookAt(player.GetChild(0));

        /*GAME OVER LOGIC*/

        Invoke("endGameOnCatch", 4f);


        /*HasRespawned = false;
        Invoke("RespawnPlayer", 2f);
        GetComponent<NunPatrol>()._isGracePeriod = true;
        player.GetComponent<Awakening>().enabled = true;*/
        return;


    }

    /* private void RespawnPlayer()
     {
         //Update the life count:

         if (!HasRespawned)
         {
             HasRespawned = true;
             //Reset the positons
             agent.Warp(nunOGpos);
             player.GetComponent<CharacterController>().enabled = false;
             player.position = playerOGpos;
             player.GetComponent<CharacterController>().enabled = true;
             nunPatrol.StartGracePeriod();

             lifeCounter.SendMessage("RecieveMessageCatchPlayer");

         }
     }*/


    void endGameOnCatch()
    {

        Debug.Log("GAME IS NOW OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
