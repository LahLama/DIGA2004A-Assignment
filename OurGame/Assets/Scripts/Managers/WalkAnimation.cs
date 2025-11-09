using UnityEngine;

public class WalkAnimation : MonoBehaviour
{
    public Animator MoveAnimation;

    void Update()
    {

        if (GameObject.FindAnyObjectByType<PlayerMovement>()._moveInput.magnitude > 0.1f && PlayerStats.Instance.playerLevel != PlayerStats.PlayerLevel.Cutscene)
        {
            MoveAnimation.SetBool("isMovin", true);
        }
        else
        {
            MoveAnimation.SetBool("isMovin", false);
        }
    }
}
