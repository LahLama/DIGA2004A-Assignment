using UnityEngine;

public class interactionAnimation : MonoBehaviour, IInteractables
{

    public void Interact()
    {
        if (TryGetComponent<Animator>(out Animator animator))
            animator.SetTrigger("trigAnim");
        else if (transform.parent.TryGetComponent<Animator>(out Animator Parentanimator))
            Parentanimator.SetTrigger("trigAnim");
    }
}
