using UnityEngine;

public class interactionAnimation : MonoBehaviour, IInteractables
{
    public void Interact()
    {
        this.GetComponent<Animator>().SetTrigger("trigAnim");
    }
}
