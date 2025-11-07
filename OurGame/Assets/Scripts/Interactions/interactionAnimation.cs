using UnityEngine;

public class interactionAnimation : MonoBehaviour, IInteractables
{

    public void Interact()
    {
        if (TryGetComponent<Animator>(out Animator animator))
            animator.SetTrigger("trigAnim");
        else
            GetComponentInChildren<Animator>().SetTrigger("trigAnim");
    }
}
