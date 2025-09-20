using UnityEngine;

public class Tutorial : MonoBehaviour
{

    private NunAi nunBaseScript;
    private VignetteControl VC;
    private Transform player;



    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void EndTutorial()
    {
        if (GameObject.FindGameObjectWithTag("NunEnemy").TryGetComponent<NunAi>(out nunBaseScript))
        {
            nunBaseScript.enabled = true;
        }


        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;

        VC.RemoveVignette(0);
    }
}
