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
    private GameObject BaseGameBranch;
    [SerializeField] private Vector3 OGplayerPos;



    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        TutorialBranch = this.gameObject;

        BaseGameBranch = GameObject.FindGameObjectWithTag("BaseGame");
        BaseGameBranch.SetActive(false);
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



        BaseGameBranch.SetActive(true);


        //This must be the last line
        TutorialBranch.SetActive(false);
        return;
    }
}
