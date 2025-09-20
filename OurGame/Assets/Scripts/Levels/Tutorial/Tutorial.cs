using UnityEngine;
using UnityEngine.TextCore.Text;

public class Tutorial : MonoBehaviour
{

    private NunAi nunBaseScript;
    private TutorialNunAI tutorialNun;
    private VignetteControl VC;
    private Transform player;
    private PlayerStats playerStats;
    private GameObject TutorialBranch;
    [SerializeField] private Vector3 OGplayerPos;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        TutorialBranch = GameObject.FindGameObjectWithTag("Tutorial");
        tutorialNun = GameObject.FindGameObjectWithTag("NunEnemy").GetComponent<TutorialNunAI>();
        VC = GameObject.Find("VignetteControl").GetComponent<VignetteControl>();


        OGplayerPos = player.transform.localPosition;
    }
    public void EndTutorial()
    {
        playerStats.playerLevel = PlayerStats.PlayerLevel.BaseGame;



        if (GameObject.FindGameObjectWithTag("NunEnemy").TryGetComponent<NunAi>(out nunBaseScript))
        {
            nunBaseScript.enabled = true;
            tutorialNun.enabled = false;
        }
        tutorialNun.gameObject.transform.position = tutorialNun.OriginalPos;

        CharacterController cc = player.gameObject.GetComponent<CharacterController>();



        cc.enabled = false;
        player.position = OGplayerPos + Vector3.up;
        cc.enabled = true;


        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;

        VC.RemoveVignette(0);

        TutorialBranch.SetActive(false);
        return;
    }
}
