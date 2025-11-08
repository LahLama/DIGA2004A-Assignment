using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// External references for typewriter effect and dialogue systems
// https://www.youtube.com/watch?v=UR_Rh0c4gbY
// https://howto.im/q/how-to-create-a-typewriter-effect-with-textmesh-pro-in-unity
// https://docs.unity3d.com/Manual/index.html
// https://generalistprogrammer.com/tutorials/unity-dialogue-system-complete-conversation-tutorial

public class OpeningCutsceneUI : MonoBehaviour
{
    public CanvasGroup blackScreen;               // UI overlay for fade-in/out effect
    public TextMeshProUGUI dialogueText;          // Text element for displaying monologue
    public AudioSource audioSource;               // AudioSource for typing sound
    public AudioClip typingSound;                 // Sound played during typewriter effect

    public GameObject UIPanel;                    // UI panel to activate after cutscene

    [TextArea(2, 5)]
    public string[] monologueLines = new string[0]; // Array of dialogue lines for the cutscene

    public float delayBetweenLines = 3f;          // Delay between each line of dialogue
    public float typingSpeed = 0.05f;             // Speed at which characters appear

    private int currentLine = 0;                  // Tracks current line index

    void Start()
    {
        // Hide UI panel at start
        if (UIPanel != null)
            UIPanel.SetActive(false);

        // Ensure monologue array is initialized
        if (monologueLines == null)
            monologueLines = new string[0];

        // Attempt to auto-assign missing references
        if (blackScreen == null)
            blackScreen = GetComponentInChildren<CanvasGroup>();

        if (dialogueText == null)
            dialogueText = GetComponentInChildren<TextMeshProUGUI>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // If critical references are missing, log warning and disable script
        if (blackScreen == null || dialogueText == null)
        {
            Debug.LogWarning("OpeningCutsceneUI: Missing references (CanvasGroup or TextMeshProUGUI). Disabling script.");
            if (UIPanel != null)
                UIPanel.SetActive(true);
            enabled = false;
            return;
        }

        // Start the cutscene coroutine
        StartCoroutine(PlayMonologue());
    }

    IEnumerator PlayMonologue()
    {
        // Unlock and show cursor for cutscene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause game time
        Time.timeScale = 0f;

        // Set black screen fully visible and clear dialogue text
        blackScreen.alpha = 1f;
        dialogueText.text = "";

        // Optional delay before first line
        yield return new WaitForSecondsRealtime(1f);

        // Loop through each line in the monologue
        while (currentLine < monologueLines.Length)
        {
            yield return StartCoroutine(TypeLine(monologueLines[currentLine])); // Type out line
            currentLine++;
            yield return new WaitForSecondsRealtime(delayBetweenLines);         // Wait before next line
        }

        // Fade out black screen after dialogue
        yield return StartCoroutine(FadeOutBlackScreen());

        // Resume game time
        Time.timeScale = 1f;

        // Lock and hide cursor after cutscene
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Activate UI panel
        if (UIPanel != null)
            UIPanel.SetActive(true);

        // Disable cutscene UI
        gameObject.SetActive(false);

        // Update player level state
        PlayerStats.Instance.playerLevel = PlayerStats.PlayerLevel.Tutorial;
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        // Reveal each character one by one
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            // Play typing sound for non-whitespace characters
            if (audioSource != null && typingSound != null && !char.IsWhiteSpace(letter))
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f); // Slight pitch variation
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSecondsRealtime(typingSpeed); // Wait before next character
        }
    }

    IEnumerator FadeOutBlackScreen()
    {
        float duration = 2f;     // Duration of fade
        float elapsed = 0f;      // Time elapsed

        // Gradually reduce alpha from 1 to 0
        while (elapsed < duration)
        {
            blackScreen.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.unscaledDeltaTime; // Use unscaled time since game is paused
            yield return null;
        }

        blackScreen.alpha = 0f; // Ensure fully transparent
    }

    public void SkipCutscene()
    {
        // Stop all running coroutines
        StopAllCoroutines();

        // Immediately hide black screen
        blackScreen.alpha = 0f;

        // Resume game time
        Time.timeScale = 1f;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Activate UI panel
        if (UIPanel != null)
            UIPanel.SetActive(true);

        // Disable cutscene UI
        gameObject.SetActive(false);
    }
}