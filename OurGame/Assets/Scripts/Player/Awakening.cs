using Unity.VisualScripting;
using UnityEngine;

public class Awakening : MonoBehaviour
{
    Animation animationClip;
    PlayerMovement playerMovement;
    LookFunction lookFunction;
    public Material BlinkShader;
    public float BlinkVal;
    public GameObject UI;

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
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.rotation = Quaternion.identity;


        if (playerMovement != null) playerMovement.enabled = false;
        if (lookFunction != null) lookFunction.enabled = false;

        if (animationClip != null && !animationClip.isPlaying)
            animationClip.Play();

        if (BlinkShader != null)
            BlinkShader.SetFloat("_blinkState", BlinkVal);


        UI.SetActive(true);
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
