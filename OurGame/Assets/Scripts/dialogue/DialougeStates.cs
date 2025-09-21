using UnityEngine;

public class DialougeState : MonoBehaviour
{
    public GameObject DialougeContainer;
    private GameObject player;
    private GameObject tooltip;
    public DialogueManagerSO dialogueManager;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tooltip = GameObject.FindGameObjectWithTag("Tooltip");

        DialougeContainer.SetActive(false);
    }
    public void StartDialouge()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<LookFunction>().enabled = false;
        tooltip.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        DialougeContainer.SetActive(true);

        dialogueManager.StartFromOtherScript();
    }

    public void EndDialouge()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<LookFunction>().enabled = true;
        tooltip.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        DialougeContainer.SetActive(false);


    }
}
