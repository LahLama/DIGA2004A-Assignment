using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//https://www.youtube.com/watch?v=UR_Rh0c4gbY
//https://howto.im/q/how-to-create-a-typewriter-effect-with-textmesh-pro-in-unity
//https://docs.unity3d.com/Manual/index.html
//https://generalistprogrammer.com/tutorials/unity-dialogue-system-complete-conversation-tutorial

public class OpeningCutsceneUI : MonoBehaviour
{
    public CanvasGroup blackScreen;
    public TextMeshProUGUI dialogueText;
    public AudioSource audioSource;
    public AudioClip typingSound;

    public GameObject UIPanel;

    [TextArea(2, 5)]
    public string[] monologueLines = new string[0];

    public float delayBetweenLines = 3f;
    public float typingSpeed = 0.05f; // Speed of character reveal

    private int currentLine = 0;

    void Start()
    {
        if (UIPanel != null)
            UIPanel.SetActive(false);

        if (monologueLines == null)
            monologueLines = new string[0];

        if (blackScreen == null)
            blackScreen = GetComponentInChildren<CanvasGroup>();

        if (dialogueText == null)
            dialogueText = GetComponentInChildren<TextMeshProUGUI>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (blackScreen == null || dialogueText == null)
        {
            Debug.LogWarning("OpeningCutsceneUI: Missing references (CanvasGroup or TextMeshProUGUI). Disabling script.");
            if (UIPanel != null)
                UIPanel.SetActive(true);
            enabled = false;
            return;
        }

        StartCoroutine(PlayMonologue());
    }

    IEnumerator PlayMonologue()
    {
        // Unlock cursor when dialogue starts
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause the game
        Time.timeScale = 0f;

        blackScreen.alpha = 1f;
        dialogueText.text = "";

        yield return new WaitForSecondsRealtime(1f); // Optional pause before first line

        while (currentLine < monologueLines.Length)
        {
            yield return StartCoroutine(TypeLine(monologueLines[currentLine]));
            currentLine++;
            yield return new WaitForSecondsRealtime(delayBetweenLines);
        }

        yield return StartCoroutine(FadeOutBlackScreen());

        // Resume the game
        Time.timeScale = 1f;

        // Lock cursor after dialogue ends
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (UIPanel != null)
            UIPanel.SetActive(true);

        gameObject.SetActive(false); // Hide cutscene UI
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            // Play typing sound for non-whitespace characters (if available)
            if (audioSource != null && typingSound != null && !char.IsWhiteSpace(letter))
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    IEnumerator FadeOutBlackScreen()
    {
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            blackScreen.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.unscaledDeltaTime; // Use unscaled time since game is paused
            yield return null;
        }

        blackScreen.alpha = 0f;
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();
        blackScreen.alpha = 0f;

        // Resume the game
        Time.timeScale = 1f;

        // Lock cursor when skipping
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (UIPanel != null)
            UIPanel.SetActive(true);

        gameObject.SetActive(false); // Hide cutscene UI
    }
}