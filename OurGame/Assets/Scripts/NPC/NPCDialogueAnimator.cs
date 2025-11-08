using UnityEngine;

public class NPCDialogueAnimator : MonoBehaviour
{
    public Animator npcAnimator; // Drag your NPC Animator here in Inspector
 // These can be called when your dialogue starts/ends
    public void StartDialogue()
    {
        npcAnimator.SetTrigger("StartDialogue");
    }

    public void EndDialogue()
    {
        npcAnimator.SetTrigger("EndDialogue");
    }

    // Optional: Start dialogue on click (for testing)
    private void OnMouseDown()
    {
        StartDialogue();
          // Simulate end after 3 seconds (for testing)
        Invoke("EndDialogue", 3f);
    }
}