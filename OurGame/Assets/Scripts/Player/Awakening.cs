using UnityEngine;

public class Awakening : MonoBehaviour
{
    Animation animationClip;
    PlayerMovement playerMovement;
    LookFunction lookFunction;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookFunction = GetComponent<LookFunction>();
        animationClip = GetComponent<Animation>();
        playerMovement.enabled = false;
        lookFunction.enabled = false;

    }


    void Update()
    {
        if (!animationClip.isPlaying)
        {
            playerMovement.enabled = true;
            lookFunction.enabled = true;
            this.enabled = false;
        }
    }


}
