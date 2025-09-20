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
    private Transform player;
    public Vector3 OriginalPos;
    private bool TutEnd = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OriginalPos = transform.position;
        vignetteControl = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<Tutorial>();

        if (TryGetComponent<NunAi>(out nunBaseScript))
        {
            nunBaseScript.enabled = false;
        }
        this.transform.localScale = Vector3.zero;

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

            vignetteControl.ApplyVignette(1);
        }

        if (!TutEnd && Mathf.Abs(player.localPosition.magnitude - agent.transform.localPosition.magnitude) <= agent.stoppingDistance)
        {

            //wait for black screen/Animation
            Invoke("EndTut", 7);
            TutEnd = true;
        }

    }
    private void EndTut()
    {

        tutorial.EndTutorial();

    }
    public void SpawnNunOnPlayer()
    {


        this.transform.localScale = Vector3.one;

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
