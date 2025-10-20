using UnityEngine;

public class DialougeState : MonoBehaviour
{
    public GameObject DialougeContainer;
    private GameObject player;
    [SerializeField] private GameObject tooltip; // assign in Inspector when possible
    public DialogueManagerSO dialogueManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // try to use the inspector reference first, fallback to FindWithTag
        if (tooltip == null)
            TryFindTooltip();

        DialougeContainer.SetActive(false);
    }

    void Update()
    {
        // if tooltip is created/enabled later, keep attempting to find it
        if (tooltip == null)
            TryFindTooltip();
    }

    private bool TryFindTooltip()
    {
        var go = GameObject.FindGameObjectWithTag("Tooltip");
        if (go == null) return false;
        tooltip = go;
        return true;
    }

    public void StartDialouge()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;
        if (tooltip != null) tooltip.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DialougeContainer.SetActive(true);

        dialogueManager.StartFromOtherScript();
    }

    public void EndDialouge()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;
        if (tooltip != null) tooltip.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        DialougeContainer.SetActive(false);
    }
}
