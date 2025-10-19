using Unity.VisualScripting;
using UnityEngine;

public class Awakening : MonoBehaviour
{
    Animation animationClip;
    PlayerMovement playerMovement;
    LookFunction lookFunction;
    public Material BlinkShader;
    public float BlinkVal;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookFunction = GetComponent<LookFunction>();
        animationClip = GetComponent<Animation>();
        playerMovement.enabled = false;
        lookFunction.enabled = false;
        if (!animationClip.isPlaying)
            animationClip.Play();

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
