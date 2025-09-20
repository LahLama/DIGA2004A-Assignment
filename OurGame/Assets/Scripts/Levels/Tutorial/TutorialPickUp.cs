using UnityEngine;

public class TutorialPickUp : MonoBehaviour
{

    private Transform player;
    private TutorialNunAI tutorialNun;
    private bool hasBeenChecked = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorialNun = GameObject.FindGameObjectWithTag("NunEnemy").GetComponent<TutorialNunAI>();
    }
    void Update()
    {

        if (this.transform.IsChildOf(player) && !hasBeenChecked)
        {
            tutorialNun.SpawnNunOnPlayer();
            hasBeenChecked = true;
            this.gameObject.SetActive(false);
        }
    }
}
