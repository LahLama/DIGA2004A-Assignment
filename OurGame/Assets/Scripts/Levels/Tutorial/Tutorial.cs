using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class Tutorial : MonoBehaviour
{

    private NunAi nunBaseScript;
    private TutorialNunAI tutorialNun;
    private VignetteControl VC;
    private Transform player;
    private PlayerStats playerStats;
    private GameObject TutorialBranch;
    private GameObject BaseGameBranch;
    [SerializeField] private Vector3 OGplayerPos;
    private NunDoors nunDoors;
    private NunCatch nunCatch;
    private NunChase nunChase;
    private NunPatrol nunPatrol;
    private NunAi nunAi;




    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = FindAnyObjectByType<PlayerStats>();
        TutorialBranch = this.gameObject;

        BaseGameBranch = GameObject.FindGameObjectWithTag("BaseGame");
        BaseGameBranch.SetActive(false);
        tutorialNun = FindAnyObjectByType<TutorialNunAI>();
        VC = FindAnyObjectByType<VignetteControl>();

        nunDoors = FindAnyObjectByType<NunDoors>();
        nunCatch = FindAnyObjectByType<NunCatch>();
        nunChase = FindAnyObjectByType<NunChase>();
        nunPatrol = FindAnyObjectByType<NunPatrol>();
        nunAi = FindAnyObjectByType<NunAi>();

        nunDoors.enabled = false;
        nunCatch.enabled = false;
        nunChase.enabled = false;
        nunPatrol.enabled = false;
        nunAi.enabled = false;

        OGplayerPos = player.transform.localPosition;
    }
    public void EndTutorial()
    {
        playerStats.playerLevel = PlayerStats.PlayerLevel.BaseGame;


        tutorialNun.enabled = false;
        nunDoors.enabled = true;
        nunCatch.enabled = true;
        nunChase.enabled = true;
        nunPatrol.enabled = true;
        nunAi.enabled = true;

        tutorialNun.gameObject.GetComponent<NavMeshAgent>().Warp(tutorialNun.OriginalPos);

        CharacterController cc = player.gameObject.GetComponent<CharacterController>();



        cc.enabled = false;
        player.position = OGplayerPos + Vector3.up;
        cc.enabled = true;


        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;

        VC.RemoveVignette(0);



        BaseGameBranch.SetActive(true);


        //This must be the last line
        TutorialBranch.SetActive(false);
        return;
    }
}
