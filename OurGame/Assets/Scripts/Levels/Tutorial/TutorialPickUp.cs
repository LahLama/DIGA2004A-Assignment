using UnityEngine;

public class TutorialPickUp : MonoBehaviour
{

    private Transform player;
    private TutorialNunAI tutorialNun;
    public bool hasBeenPicked = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorialNun = GameObject.FindAnyObjectByType<TutorialNunAI>();
    }
    void Update()
    {

        if (this.transform.IsChildOf(player) && !hasBeenPicked)
        {
            tutorialNun.SpawnNunOnPlayer();
            hasBeenPicked = true;
            Destroy(this.gameObject);
        }
    }
}
