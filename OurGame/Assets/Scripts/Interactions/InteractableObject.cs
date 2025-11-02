using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Object Info")]
    public string objectName = "Object";         // Must match in Nun script: "Vase" or "Bookshelf"

    [Header("Interaction Settings")]
    public float interactDistance = 1f;
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPromptUI;          // Optional "Press E" text

    [Header("Animation Settings")]
    public Animator animator;
    public string animationTriggerName = "Activate"; // e.g. "Break" for vase, "Open" for bookshelf

    private Transform player;
    private bool hasInteracted = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);

        // Safety: make sure animation doesn’t auto-play
        if (animator != null)
            animator.ResetTrigger(animationTriggerName);
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool playerIsClose = distance <= interactDistance;

        // Show "Press E" prompt
        if (interactPromptUI != null)
            interactPromptUI.SetActive(playerIsClose && !hasInteracted);

        // Only trigger when close & pressing E
        if (playerIsClose && !hasInteracted && Input.GetKeyDown(interactKey))
        {
            TriggerInteraction();
        }
    }
    private void TriggerInteraction()
    {
        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            animator.SetTrigger(animationTriggerName);
        }

        hasInteracted = true;

        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);

        Debug.Log($"{objectName} interacted!");
        InteractableEvents.RaiseObjectInteracted(objectName);
    }
}