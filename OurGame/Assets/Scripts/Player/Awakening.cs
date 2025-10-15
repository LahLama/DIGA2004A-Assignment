using UnityEngine;

public class Awakening : MonoBehaviour
{
    Animation animation;
    PlayerMovement playerMovement;
    LookFunction lookFunction;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookFunction = GetComponent<LookFunction>();
        animation = GetComponent<Animation>();
        playerMovement.enabled = false;
        lookFunction.enabled = false;

    }


    void Update()
    {
        if (!animation.isPlaying)
        {
            playerMovement.enabled = true;
            lookFunction.enabled = true;
            this.enabled = false;
        }
    }


}
