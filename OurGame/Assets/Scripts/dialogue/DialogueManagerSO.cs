using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManagerSO : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button choiceButton1;
    public Button choiceButton2;
    public TextMeshProUGUI choiceText1;
    public TextMeshProUGUI choiceText2;
    public DialougeState dialougeState;

    [Header("StartNode")]
    public DialogueNodeSO startingNode;

    private DialogueNodeSO currentNode;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool waitingForEnd = false;

     public AudioSource audioSource;
    public AudioClip typingSound;

    //Called to start dialogue from another script
    public void StartFromOtherScript()
    {
        StartDialogue(startingNode);

         if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {


        if (isTyping && Input.GetMouseButtonDown(0))
        {
            // Skip typing
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentNode.dialogueLine;
            isTyping = false;
            ShowChoices();
        }
        //End dialogue when waiting for end and mouse clicked
        else if (waitingForEnd && Input.GetMouseButtonDown(0))
        {
            waitingForEnd = false;
            CallEndFromScript();
        }
    }

    //Start the dialogue from a given node
    public void StartDialogue(DialogueNodeSO startNode)
    {
        dialoguePanel.SetActive(true);
        ShowNode(startNode);
    }

    //Display the current dialogue node
    private void ShowNode(DialogueNodeSO node)
    {
        currentNode = node;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(node.dialogueLine));
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }

    //typewriter effect for dialogue text
    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in fullText)
        {
            dialogueText.text += c;

            if (audioSource != null && typingSound != null && !char.IsWhiteSpace(c))
            {
                // Slight pitch variation for more natural sound
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
        ShowChoices();
    }

    // Display choice buttons if available
    private void ShowChoices()
    {
        if (currentNode.choices.Length > 0)
        {
            choiceButton1.gameObject.SetActive(true);
            choiceText1.text = currentNode.choices[0].choiceText;

            choiceButton1.onClick.RemoveAllListeners();
            choiceButton1.onClick.AddListener(() =>
            {
                ShowNode(currentNode.choices[0].nextNode);
            });
        }

        // Second choice (if exists)
        if (currentNode.choices.Length > 1)
        {
            choiceButton2.gameObject.SetActive(true);
            choiceText2.text = currentNode.choices[1].choiceText;

            choiceButton2.onClick.RemoveAllListeners();
            choiceButton2.onClick.AddListener(() =>
            {
                ShowNode(currentNode.choices[1].nextNode);
            });
        }

        if (currentNode.choices.Length == 0)
        {
            waitingForEnd = true;
        }
    }

    //Ends the Dialogue from another script
    private void CallEndFromScript()
    {
        dialougeState.EndDialouge();
    }
    

}
