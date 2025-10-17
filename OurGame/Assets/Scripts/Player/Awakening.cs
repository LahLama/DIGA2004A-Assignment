using Unity.VisualScripting;
using UnityEngine;

public class Awakening : MonoBehaviour
{
    Animation animationClip;
    PlayerMovement playerMovement;
    LookFunction lookFunction;
    public Material BlinkShader;
    public float BlinkVal;

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
        BlinkShader.SetFloat("_blinkState", BlinkVal);

        if (!animationClip.isPlaying)
        {
            playerMovement.enabled = true;
            lookFunction.enabled = true;
            BlinkShader.SetFloat("_blinkState", BlinkVal);
            this.enabled = false;



        }
    }


}
