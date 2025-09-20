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


        OGplayerPos = player.transform.position;
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

        CharacterController CC = player.gameObject.GetComponent<CharacterController>();
        CC.enabled = false;
        player.position = new Vector3(OGplayerPos.x, OGplayerPos.y + 1, OGplayerPos.z);
        CC.enabled = true;

        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;

        VC.RemoveVignette(0);

        TutorialBranch.SetActive(false);
    }
}
