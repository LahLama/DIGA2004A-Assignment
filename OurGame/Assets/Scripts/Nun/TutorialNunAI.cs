using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TutorialNunAI : MonoBehaviour
{
    private NunAi nunBaseScript;
    private NavMeshAgent agent;
    private VignetteControl vignetteControl;
    private bool nunSpawned = false;
    private Tutorial tutorial;
    private TutorialPickUp tutorialPickUp;
    private Transform player;
    public Vector3 OriginalPos;
    private bool TutEnded = false, TutItem = false, ApplyVignette = false;
    [SerializeField] private Transform NunSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginalPos = transform.position;
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<Tutorial>();
        tutorialPickUp = tutorial.gameObject.transform.GetChild(0).GetComponent<TutorialPickUp>();

        if (TryGetComponent<NunAi>(out nunBaseScript))
        {
            nunBaseScript.enabled = false;
        }
        this.transform.localScale = Vector3.one;

    }

    private void Update()
    {
        if (nunSpawned)
        {

            player.GetComponentInChildren<Camera>().transform.LookAt(
                    new Vector3(
                        this.gameObject.transform.position.x,
                        this.gameObject.transform.position.y + agent.height,
                         this.gameObject.transform.position.z
                         )

                        );
            agent.transform.LookAt(player.GetChild(0));
            if (ApplyVignette == false)
            {
                vignetteControl.ApplyVignette(1);
                ApplyVignette = true;
            }

        }
        TutItem = tutorialPickUp.hasBeenPicked;
        float dist = Vector3.Distance(player.position, agent.transform.position);


        if (TutItem && !TutEnded && dist <= agent.stoppingDistance)
        {
            Debug.Log("DEBUG 1");
            //wait for black screen/Animation
            Invoke("EndTut", 2);
            TutEnded = true;
        }
    }
    private void EndTut()
    {

        tutorial.EndTutorial();
        vignetteControl.RemoveVignette(1);
        nunBaseScript.StartGracePeriod();

    }
    public void SpawnNunOnPlayer()
    {


        this.transform.localScale = Vector3.one;

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;


        //Warp() is the correct way to instantly move a NavMeshAgent.
        agent.Warp(NunSpawnPoint.position);

        nunSpawned = true;

        Invoke("MoveToPlayer", 2);
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(player.position);


    }

}
